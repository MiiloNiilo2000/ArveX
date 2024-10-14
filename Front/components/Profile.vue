<template>
<div class="relative max-w-xl p-8">
  <div class="flex space-x-4">
    <button @click="addUser" class="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600">
      Lisa kasutaja
    </button>
    
    <button @click="addCompany" class="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600">
      Lisa ettevõte
    </button>
  </div>
</div>

    <div class="max-w-xl p-8">
      <h1 class="text-3xl font-bold mb-6">{{ title }}</h1>
  
      <div class="bg-green-100 shadow-md rounded-lg p-6">
        <div class="flex items-center">
          <img :src="userProfile.image" alt="Profile Picture" class="rounded-full w-24 h-24 mr-6" />
          <div>
            <h2 class="text-xl text-black font-semibold">{{ userProfile.username }}</h2>
            <p class="text-gray-600">{{ userProfile.email }}</p>
          </div>
        </div>
  
        <div class="mt-6">
          <h3 class="text-lg text-black font-medium">Bio</h3>
          <p class="text-gray-800 mt-2">{{ userProfile.bio }}</p>
        </div>
  
        <div class="mt-6">
          <UButton @click="editProfile" class="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600">Muuda profiili</UButton>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  const title = 'Kasutaja profiil'
  
  import { useProfileStore } from '~/stores/profileStores';
  const profileStore = useProfileStore();
  const userProfile = computed(() => profileStore.profiles[0]);
  import { useRouter } from 'vue-router';
  const router = useRouter();

  const editProfile = () => {
    alert('Profile editing not implemented yet!')
  }
  const addUser = () => {
    router.push('/add-profile')
  }
  const addCompany = () => {
    router.push('/add-company')
  }
  </script>
  
  <style scoped>
  body {
    background-color: #f3f4f6;
  }
  </style>