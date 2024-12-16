<template>
    <div class="flex justify-center items-center pt-9">
      <div class="w-96 p-6 flex flex-col h-99">
        <h2 class="text-2xl font-bold mb-4 text-center">Muuda parooli</h2>
        <UForm
          :state="passwordForm"
          class="space-y-4"
          @submit="onPasswordSubmit"
          @error="onError"
        >
          <UFormGroup label="Vana parool" name="oldPassword">
            <UInput
              v-model="passwordForm.oldPassword"
              type="password"
              color="emerald"
              class="bg-gray-900 rounded-md"
            />
          </UFormGroup>
          <UFormGroup label="Uus parool" name="newPassword">
            <UInput
              v-model="passwordForm.newPassword"
              type="password"
              color="emerald"
              class="bg-gray-900 rounded-md"
            />
          </UFormGroup>
          <UFormGroup label="Kinnita uus parool" name="confirmPassword">
            <UInput
              v-model="passwordForm.confirmPassword"
              type="password"
              color="emerald"
              class="bg-gray-900 rounded-md"
            />
          </UFormGroup>
  
          <div class="flex justify-between mt-4">
            <UButton type="submit" :disabled="isSubmitDisabled">Muuda parooli</UButton>
            <UButton @click="cancelChangePassword" color="gray">Tühista</UButton>
          </div>
  
          <div v-if="passwordForm.newPassword" class="space-y-2 text-sm mt-4">
            <div :class="passwordRequirements.isValid ? 'text-green-500' : 'text-red-500'">
              <span v-if="passwordRequirements.isValid">✔</span>
              Parool peab olema vähemalt 8 tähemärki pikk.
            </div>
            <div :class="hasDigit ? 'text-green-500' : 'text-red-500'">
              <span v-if="hasDigit">✔</span>
              Parool peab sisaldama vähemalt ühte numbrit.
            </div>
            <div :class="hasLower ? 'text-green-500' : 'text-red-500'">
              <span v-if="hasLower">✔</span>
              Parool peab sisaldama vähemalt ühte väikest tähte.
            </div>
            <div :class="hasUpper ? 'text-green-500' : 'text-red-500'">
              <span v-if="hasUpper">✔</span>
              Parool peab sisaldama vähemalt ühte suurt tähte.
            </div>
            <div :class="hasSpecialChar ? 'text-green-500' : 'text-red-500'">
              <span v-if="hasSpecialChar">✔</span>
              Parool peab sisaldama vähemalt ühte eri märki näiteks # / ! " % &.
            </div>
          </div>
  
          <div v-if="message" :class="messageType === 'error' ? 'text-red-500' : 'text-green-500'" class="mt-4">
            {{ message }}
          </div>
        </UForm>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, computed, onMounted } from 'vue';
  import { useRouter } from 'vue-router';
  import axios from 'axios';
  
  const passwordForm = ref({
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  
  const message = ref('');
  const messageType = ref('');
  const router = useRouter();
  
  const hasDigit = computed(() => /\d/.test(passwordForm.value.newPassword));
  const hasLower = computed(() => /[a-z]/.test(passwordForm.value.newPassword));
  const hasUpper = computed(() => /[A-Z]/.test(passwordForm.value.newPassword));
  const hasSpecialChar = computed(() => /[^a-zA-Z0-9]/.test(passwordForm.value.newPassword));
  const isPasswordLengthValid = computed(() => passwordForm.value.newPassword.length >= 8);
  
  const passwordRequirements = computed(() => ({
    isValid: isPasswordLengthValid.value && hasDigit.value && hasLower.value && hasUpper.value && hasSpecialChar.value,
    errorMessage: isPasswordLengthValid.value ? 
                  (hasDigit.value ? 
                   (hasLower.value ? 
                    (hasUpper.value ? 
                     (hasSpecialChar.value ? '' : 'Parool peab sisaldama mitte-alfa numbrilisi märke.') 
                     : 'Parool peab sisaldama vähemalt ühte suurt tähte.') 
                    : 'Parool peab sisaldama vähemalt ühte väikest tähte.') 
                   : 'Parool peab sisaldama vähemalt ühte numbrit.') 
                  : 'Parool peab olema vähemalt 8 tähemärki pikk.'
  }));
  
  const passwordsMatch = computed(() => {
    return passwordForm.value.newPassword === passwordForm.value.confirmPassword;
  });
  
  const isSubmitDisabled = computed(() => {
    return !passwordRequirements.value.isValid || !passwordsMatch.value;
  });
  
  const onPasswordSubmit = async () => {
    if (!passwordRequirements.value.isValid) {
      message.value = passwordRequirements.value.errorMessage;
      messageType.value = 'error';
      return;
    }
  
    if (!passwordsMatch.value) {
      message.value = 'Uus parool ja parooli kinnitus ei ühti!';
      messageType.value = 'error';
      return;
    }
  
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        message.value = 'Palun logige sisse!';
        messageType.value = 'error';
        router.push('/login');
        return;
      }
  
      const response = await axios.put('http://localhost:5176/Profile/UpdatePassword', {
        oldPassword: passwordForm.value.oldPassword,
        newPassword: passwordForm.value.newPassword
      }, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
  
      if (response.status === 200) {
        localStorage.removeItem('token');
        router.push('/login');
        location.reload();
      } else {
        throw new Error('Parooli uuendamine ebaõnnestus!');
      }
    } catch (error) {
      console.error('Parooli uuendamise viga:', error);
      message.value = 'Parooli uuendamine ebaõnnestus!';
      messageType.value = 'error';
    }
  };
  
  onMounted(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      router.push('/login');
    }
  });
  
  function onError(event: ErrorEvent) {
    const element = document.getElementById(event.error[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }
  
  const cancelChangePassword = () => {
    router.push('/edit-profile');
  };
  </script>
  
  <style scoped>
  </style>
  