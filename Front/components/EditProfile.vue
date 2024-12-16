<template>
  <div class="flex justify-center items-center pt-9">
    <div class="w-96 p-6 flex flex-col h-99">
      <h2 class="text-2xl font-bold mb-4 text-center">Muuda profiili</h2>
      <UForm
        :state="form"
        class="space-y-4"
        @submit="prepareUpdateProfile"
        @error="onError"
      >
        <UFormGroup label="Kasutajanimi" name="username">
          <UInput
            v-model="form.username"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <UFormGroup label="E-post" name="email">
          <UInput
            v-model="form.email"
            color="emerald"
            class="bg-gray-900 rounded-md"
          />
        </UFormGroup>
        <div class="col-span-2 flex justify-center">
          <UButton type="submit"> Salvesta </UButton>
        </div>
      </UForm>

      <div v-if="showModal" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
        <div class="bg-white p-6 rounded-lg shadow-lg">
          <h3 class="text-xl font-semibold">Tähelepanu</h3>
          <p class="mt-2">Pärast profiili muutmist tuleb teil uuesti sisse logida. Kas soovite jätkata?</p>
          <div class="mt-4 flex justify-end">
            <UButton @click="updateProfile" class="mr-2" color="green">Jah, logi välja</UButton>
            <UButton @click="cancelLogout" color="red">Ei, jää sisse</UButton>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { useRouter } from 'vue-router';
  
  const form = ref({
    username: '',
    email: ''
  });
  
  const message = ref('');
  const messageType = ref('');
  const router = useRouter();
  const { customFetch } = useApi();
  
  const profile = ref<{
    username?: string;
    email?: string;
    image?: string;
  } | null>(null);
  
  const showModal = ref(false);
  
  const loadProfileData = async () => {
    try {
      const response = await customFetch<{
        token: string,
        id: string,
        username?: string,
        email?: string
      }>('Profile/Me', {
        method: 'GET',
      });
      profile.value = response;
  
      if (profile.value) {
        form.value.username = profile.value.username || '';
        form.value.email = profile.value.email || '';
      }
    } catch (error) {
      console.error('Error fetching profile:', error);
      profile.value = null;
    }
  };
  
  const prepareUpdateProfile = () => {
    showModal.value = true;
  };
  
  const updateProfile = async () => {
    message.value = '';
    try {
      const token = localStorage.getItem('token');
  
      if (!token) {
        message.value = 'Palun logige sisse!';
        messageType.value = 'error';
        return;
      }
      const response = await axios.put('http://localhost:5176/Profile/Update', {
        username: form.value.username,
        email: form.value.email
      }, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      if (response.status === 200) {
        message.value = 'Profiil on edukalt uuendatud!';
        messageType.value = 'success';
  
        form.value.username = response.data.userName;
        form.value.email = response.data.email;
  
        localStorage.removeItem('token');
        router.push('/login');
      } else {
        throw new Error('Profiili uuendamine ebaõnnestus!');
      }
    } catch (error) {
      console.error('Profiili uuendamise viga:', error);
      message.value = 'Profiili uuendamine ebaõnnestus!';
      messageType.value = 'error';
    } 
  };
  const cancelLogout = () => {
    showModal.value = false;
  };
  onMounted(() => {
    loadProfileData();
  });
  
  function onError(event: ErrorEvent) {
  const element = document.getElementById(event.error[0].id);
  element?.focus();
  element?.scrollIntoView({ behavior: "smooth", block: "center" });
}
  </script>
  
  <style scoped>
  select {
    border: 1.5px solid #38a169;
    background-color: #111827;
    color: white;
    border-radius: 0.375rem;
    padding: 0.2rem 0.75rem;
    font-size: 1rem;
    transition: border-color 0.2s ease-in-out;
  }
  
  select:focus {
    border-color: #42ac4e;
    outline: none;
    box-shadow: 0 0 0 0.1rem rgba(66, 248, 56, 0.413);
  }
  
  .form-field {
    border: 1px solid #38a169;
    background-color: #121212;
    color: black;
    border-radius: 0.375rem;
    padding: 0.375rem 0.75rem;
    font-size: 1rem;
    width: 100%;
    height: 3rem;
    transition: border-color 0.2s ease-in-out;
  }
  
  .form-field:focus {
    border-color: #2f855a;
    outline: none;
    box-shadow: 0 0 0 0.2rem rgba(56, 189, 248, 0.25);
  }
  </style>
  