<template>
    <div class="max-w-xl mx-auto p-8 bg-white shadow-md rounded-lg">
      <h1 class="text-3xl font-bold mb-6">Lisa Profiil</h1>
  
      <form @submit.prevent="submitProfile">
        <div class="mb-4">
          <label for="username" class="block text-sm font-medium text-gray-700">Kasutajanimi</label>
          <input type="text" id="username" v-model="username" required class="mt-1 block w-full border border-gray-300 rounded-md p-2" />
        </div>
  
        <div class="mb-4">
          <label for="email" class="block text-sm font-medium text-gray-700">E-post</label>
          <input type="email" id="email" v-model="email" required class="mt-1 block w-full border border-gray-300 rounded-md p-2" />
        </div>
  
        <div class="mb-4">
          <label for="bio" class="block text-sm font-medium text-gray-700">Bio</label>
          <textarea id="bio" v-model="bio" required class="mt-1 block w-full border border-gray-300 rounded-md p-2"></textarea>
        </div>
  
        <div class="mb-4">
          <label for="image" class="block text-sm font-medium text-gray-700">Pildi URL</label>
          <input type="text" id="image" v-model="image" required class="mt-1 block w-full border border-gray-300 rounded-md p-2" />
        </div>
  
        <button type="submit" class="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600">
          Lisa Profiil
        </button>
      </form>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import { useProfileStore } from '~/stores/profileStores';
  
  const profileStore = useProfileStore();
  
  const username = ref('');
  const email = ref('');
  const bio = ref('');
  const image = ref('');
  
  const submitProfile = () => {
    const newProfile = {
      username: username.value,
      email: email.value,
      bio: bio.value,
      image: image.value,
      id: 0 // id will be set in the store
    };
  
    profileStore.addProfile(newProfile);
  
    // Reset the form fields
    username.value = '';
    email.value = '';
    bio.value = '';
    image.value = '';
    
    alert('Profiil on lisatud!');
  }
  </script>
  
  <style scoped>
  body {
    background-color: #f3f4f6;
  }
  </style>