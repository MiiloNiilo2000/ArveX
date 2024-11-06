<template>
    <div class="max-w-full p-8">
      <h1 class="text-3xl font-bold mb-6">{{ title }}</h1>
  
      <div class="bg-green-100 shadow-md rounded-lg p-6">
        <div class="flex items-center">
          <img :src="profileImage" alt="Profile Picture" class="rounded-full w-24 h-24 mr-6" />
          <div>
            <h2 class="text-xl text-black font-semibold">{{ profile.username }}</h2>
            <p class="text-gray-600">{{ profile.email }}</p>
          </div>
        </div>
  
        <div class="mt-6">
          <h3 class="text-lg text-black font-medium">Bio</h3>
          <p class="text-gray-800 mt-2">{{ profile.bio }}</p>
        </div>
  
        <div class="mt-6">
          <UButton @click="editProfile" class="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600">
            Muuda profiili
          </UButton>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { computed, defineProps } from 'vue';
  import { useProfileStore } from '../stores/profileStores';
 
  const profileStore = useProfileStore();
  const profileImage = computed(() =>
  props.profile.image
    ? props.profile.image
    : 'https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o='
);

   const title = 'Kasutaja profiil'
  
  const props = defineProps<{
    profile: {
    image: string;
    username: string;
    email: string;
    bio: string;
  };
  
  editProfile: () => void;
}>();
</script>
  
  <style scoped>
  </style>