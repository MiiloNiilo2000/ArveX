<template>
  <div class="bg-gray-100 min-h-screen flex items-center justify-center pt-16">
    <div class="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
      <h2 class="text-3xl font-bold text-center text-blue-600 mb-6">Registreeri konto</h2>
      
      <form @submit.prevent="handleRegister" class="space-y-4">
        <div>
          <label for="username" class="block text-sm font-semibold text-gray-700 mb-2">Kasutajanimi</label>
          <input
            type="text"
            id="username"
            v-model="username"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Sisesta oma kasutajanimi"
          />
        </div>

        <div>
          <label for="email" class="block text-sm font-semibold text-gray-700 mb-2">E-post</label>
          <input
            type="email"
            id="email"
            v-model="email"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Sisesta oma e-posti aadress"
          />
        </div>

        <div>
          <label for="password" class="block text-sm font-semibold text-gray-700 mb-2">Parool</label>
          <input
            type="password"
            id="password"
            v-model="password"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Sisesta oma parool"
          />
        </div>

        <div>
          <label for="confirmPassword" class="block text-sm font-semibold text-gray-700 mb-2">Kinnita parool</label>
          <input
            type="password"
            id="confirmPassword"
            v-model="confirmPassword"
            required
            class="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Kinnita oma parool"
          />
        </div>

        <div>
          <button
            type="submit"
            :disabled="!isFormValid"
            class="w-full py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            Registreeri
          </button>
        </div>
      </form>

      <p class="text-center text-sm text-gray-600 mt-4">
        Juba konto olemas? 
        <NuxtLink to="/login" class="text-blue-600 hover:underline">Logi sisse</NuxtLink>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed } from 'vue';
  import { useRouter } from 'vue-router';
  import axios from 'axios';

  const username = ref('');
  const email = ref('');
  const password = ref('');
  const confirmPassword = ref('');
  const router = useRouter();

  const isFormValid = computed(() => {
    return username.value && email.value && password.value && password.value === confirmPassword.value;
  });

  const handleRegister = async () => {
    try {
      const response = await axios.post('http://localhost:5176/Account/register', {
        username: username.value,
        email: email.value,
        password: password.value,
      });

      if (response.status === 200) {
        router.push('/login');
        alert('Registreerimine õnnestus!');
      } else {
        alert('Registreerimine ebaõnnestus: ' + (response.data.message || 'Tundmatu viga'));
      }
    } catch (error) {
      console.error('Registreerimine ebaõnnestus', error);
      alert('Esines viga registreerimisel.');
    }
  };
</script>

<style scoped>
</style>
