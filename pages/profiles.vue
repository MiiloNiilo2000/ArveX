<template>
    <div class="max-w-xl p-8"> <!-- Eemaldame mx-auto -->
      <h1 class="text-3xl font-bold mb-6">{{ title }}</h1>
  
      <div class="bg-white shadow-md rounded-lg p-6">
        <div class="flex items-center">
          <img :src="userProfile.image" alt="Profile Picture" class="rounded-full w-24 h-24 mr-6" />
          <div>
            <h2 class="text-xl font-semibold">{{ userProfile.name }}</h2>
            <p class="text-gray-600">{{ userProfile.email }}</p>
          </div>
        </div>
  
        <div class="mt-6">
          <h3 class="text-lg font-medium">Bio</h3>
          <p class="text-gray-800 mt-2">{{ userProfile.bio }}</p>
        </div>
  
        <div class="mt-6">
          <UButton @click="editProfile" class="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600">Edit Profile</UButton>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  const title = 'User Profile'
  import { ref } from 'vue'
  
  const userProfile = ref({
    name: 'Sviit Hõum',
    email: 'sviit.houm@example.com',
    image: 'https://via.placeholder.com/150',
    bio: 'Tere, tere kõik see pere.'
  })
  
  const editProfile = () => {
    alert('Profile editing not implemented yet!')
  }
  </script>
  
  <style scoped>
  body {
    background-color: #f3f4f6;
  }
  </style>