<template>
    <div class="max-w-md mx-auto mt-10 p-6 bg-white rounded-lg shadow-md">
      <h2 class="text-2xl font-semibold mb-4">Muuda profiili</h2>
  
      <div v-if="message" :class="messageType === 'success' ? 'text-green-600' : 'text-red-600'" class="mb-4">
        {{ message }}
      </div>
  
      <form @submit.prevent="prepareUpdateProfile" class="space-y-4">
        <div>
          <label for="username" class="block text-sm font-medium text-gray-700">Kasutajanimi</label>
          <input
            id="username"
            v-model="form.username"
            type="text"
            class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            :disabled="loading"
          />
        </div>
  
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700">E-post</label>
          <input
            id="email"
            v-model="form.email"
            type="email"
            class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            :disabled="loading"
          />
        </div>
  
        <div class="flex justify-center mt-4">
          <button
            type="submit"
            class="px-6 py-2 bg-blue-600 text-white rounded-lg shadow-md hover:bg-blue-700 disabled:bg-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
            :disabled="loading"
          >
            {{ loading ? 'Salvestamine...' : 'Salvesta muudatused' }}
          </button>
        </div>
      </form>
  
      <div v-if="showModal" class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
        <div class="bg-white p-6 rounded-lg shadow-lg">
          <h3 class="text-xl font-semibold">Tähelepanu</h3>
          <p class="mt-2">Pärast profiili muutmist tuleb teil uuesti sisse logida. Kas soovite jätkata?</p>
          <div class="mt-4 flex justify-end">
            <button @click="updateProfile" class="bg-green-600 text-white py-2 px-4 rounded mr-2 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500">Jah, logi välja</button>
            <button @click="cancelLogout" class="bg-red-600 text-white py-2 px-4 rounded hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500">Ei, jää sisse</button>
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
  
  const loading = ref(false);
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
    loading.value = true;
    message.value = '';
  
    try {
      const token = localStorage.getItem('token');
  
      if (!token) {
        message.value = 'Palun logige sisse!';
        messageType.value = 'error';
        loading.value = false;
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
    } finally {
      loading.value = false;
    }
  };
  const cancelLogout = () => {
    showModal.value = false;
  };
  onMounted(() => {
    loadProfileData();
  });
  </script>
  
  <style scoped>
  form {
    width: 100%;
  }
  
  input {
    width: 100%;
    padding: 8px;
  }
  
  button {
    width: auto;
    background-color: #1D4ED8;
    color: white;
  }
  
  button:disabled {
    background-color: #d1d5db;
    color: #6b7280;
  }
  
  button:hover {
    background-color: #2563eb;
  }
  
  button:focus {
    outline: none;
    box-shadow: 0 0 0 4px rgba(37,99,235,0.4);
  }
  </style>
  