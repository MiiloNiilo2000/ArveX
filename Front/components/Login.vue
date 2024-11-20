<template>
  <div class="bg-gray-100 min-h-screen flex items-center justify-center pt-16">
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
  import { ref } from 'vue';
  import { useRouter } from 'vue-router';
  import axios from 'axios';
  
  const username = ref('');
  const password = ref('');
  const router = useRouter();
  
  const handleSubmit = async () => {
    try {
      const response = await axios.post('http://localhost:5176/Account/login', {
        username: username.value,
        password: password.value,
      });
      console.log('Username: ',username.value)
      console.log('Password: ',password.value)
      console.log('Response.data: ',response.data)
      if (response.status === 200) {
        const token = response.data;
        localStorage.setItem('token', token);
        axios.defaults.headers['Authorization'] = `Bearer ${token}`;
        router.push('/profiles');
      } 
    } 
    catch (error) {
      console.error('Sisselogimine ebaõnnestus', error);
      alert('Vale kasutajanimi või parool!');
    }
  };
  </script>
  
  <style scoped>
  </style>
  