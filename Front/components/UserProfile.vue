<template>
  <div class="max-w-full p-8">
    <h1 class="text-3xl font-bold mb-6">{{ title }}</h1>

    <div v-if="profile" class="bg-green-100 shadow-md rounded-lg p-6">
      <div class="flex items-center">
        <img 
          :src="profileImage" 
          alt="Profile Picture" 
          class="rounded-full w-24 h-24 mr-6" 
        />
        <div>
          <h2 v-if="profile.username" class="text-xl text-black font-semibold">
            {{ profile.username }}
          </h2>
          <p v-else class="text-gray-600 italic">Kasutajanimi puudub</p>

          <p v-if="profile.email" class="text-gray-600">{{ profile.email }}</p>
          <p v-else class="text-gray-600 italic">E-post puudub</p>
        </div>
      </div>

      <div class="mt-6"></div>

      <div class="mt-6">
        <UButton 
          @click="editProfile" 
          class="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600">
          Muuda profiili
        </UButton>
      </div>
    </div>

    <div v-else>
      <p class="text-red-500">Profiili andmed pole saadaval.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, defineProps } from 'vue';

const props = defineProps<{
  profile: {
    image?: string;
    username?: string;
    email?: string;
  } | null;
  editProfile: () => void;
}>();

const profileImage = computed(() =>
  props?.profile?.image
    ? props.profile.image
    : 'https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o='
);

const title = 'Kasutaja profiil';
</script>

<style scoped>
</style>
