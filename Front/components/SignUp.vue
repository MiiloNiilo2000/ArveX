<template>
  <div class="min-h-screen flex items-center justify-center pt-0">
    <div class="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
      <h2 class="text-3xl font-bold text-center text-green-600 mb-6">Registreeri konto</h2>
      
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
            class="w-full py-2 bg-green-600 text-white font-bold rounded-md hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500"
          >
            Registreeri
          </button>
        </div>
      </form>

      <p class="text-center text-sm text-gray-600 mt-4">
        Konto juba olemas? 
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
  const { customFetch } = useApi();

  const isFormValid = computed(() => {
    return username.value && email.value && password.value && password.value === confirmPassword.value;
  });

  const handleRegister = async () => {
    try {
    const response = await customFetch('Account/register', {
      method: 'POST',
      body: {
        username: username.value,
        email: email.value,
        password: password.value,
      },
    });
      if (response) {
          router.push('/login');
          alert('Registreerimine õnnestus!');
        } else {
          alert('Registreerimine ebaõnnestus: Tundmatu viga');
        }
      } catch (error: any) {
        console.error('Registreerimine ebaõnnestus', error);
        alert('Esines viga registreerimisel:' + (error?.data?.message || 'Tundmatu viga'));
      }
  };
</script>

<style scoped>
</style>
