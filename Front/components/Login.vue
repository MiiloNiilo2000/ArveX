<template>
  <div class="bg-gray-100 min-h-screen flex items-center justify-center pt-0">
    <div class="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
      <h2 class="text-3xl font-bold text-center text-blue-600 mb-6">Logi sisse</h2>
      
      <form @submit.prevent="handleSubmit" class="space-y-4">
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
          <button
            type="submit"
            class="w-full py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            Logi sisse
          </button>
        </div>
      </form>

      <p class="text-center text-sm text-gray-600 mt-4">
        Ei ole veel kontot? 
        <NuxtLink to="/signup" class="text-blue-600 hover:underline">Registreeri</NuxtLink>
      </p>
    </div>
  </div>
</template>
  
<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';

const username = ref('');
const password = ref('');
const router = useRouter();
const { customFetch } = useApi();

onMounted(() => {
  const token = localStorage.getItem('token');
  if (token) {
    router.push('/profiles');
  }
});

const handleSubmit = async () => {
  try {
    const response = await customFetch<{ token: string }>('Account/login', {
      method: 'POST',
      body: {
        username: username.value,
        password: password.value,
      },
    });

    console.log('Serveri vastus:', response);

    if (response?.token) {
      const token = response.token;

      localStorage.setItem('token', token);

      setTimeout(() => {
        router.push('/profiles');
        location.reload();
      }, 100);
    } else {
      throw new Error('Token puudub serveri vastuses!');
    }
  } catch (error: any) {
    console.error('Sisselogimine ebaõnnestus:', error);
    alert('Vale kasutajanimi või parool!');
  }
};
</script>
  
<style scoped>
</style>
