<template>
  <div class="max-w-full p-8 bg-gray-50 min-h-screen flex justify-center items-center">
    <div class="w-full max-w-3xl bg-white shadow-lg rounded-lg p-8">
      <h1 class="text-4xl font-bold text-center mb-6 text-gray-800">{{ title }}</h1>

      <div v-if="profile" class="bg-gradient-to-r from-green-100 via-green-50 to-green-100 shadow-inner rounded-lg p-8">
        <div class="flex items-center">
          <img 
            :src="profileImage" 
            alt="Profile Picture" 
            class="rounded-full border-4 border-blue-500 w-32 h-32 mr-6 shadow-lg" 
          />
          <div>
            <h2 v-if="profile.username" class="text-2xl text-gray-800 font-semibold">
              {{ profile.username }}
            </h2>
            <p v-else class="text-gray-500 italic">Kasutajanimi puudub</p>

            <p v-if="profile.email" class="text-gray-600 mt-2">{{ profile.email }}</p>
            <p v-else class="text-gray-500 italic mt-2">E-post puudub</p>
          </div>
        </div>

        <div class="mt-6 text-center">
          <UButton 
            @click="editProfile" 
            class="bg-blue-500 text-white font-semibold text-lg px-6 py-3 rounded-md hover:bg-blue-600 transition duration-200 shadow-md">
            ✎ Muuda profiili
          </UButton>
        </div>
      </div>

      <div v-else class="text-center text-red-500 mt-6">
        <p>Profiili andmed pole saadaval.</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';

const profile = ref<{
  username?: string;
  email?: string;
  image?: string;
} | null>(null);

const title = 'Kasutaja profiil';
const { customFetch } = useApi();

const profileImage = computed(() =>
  profile.value?.image ||
  'https://media.istockphoto.com/id/1300845620/vector/user-icon-flat-isolated-on-white-background-user-symbol-vector-illustration.jpg?s=612x612&w=0&k=20&c=yBeyba0hUkh14_jgv1OKqIH0CCSWU_4ckRkAoy2p73o='
);

const fetchProfile = async () => {
  try {
    const response = await customFetch<{
      token: string,
      id: string;
      username?: string;
      email?: string;
    }>('Profile/Me', {
      method: 'GET',
    });
    profile.value = response;
  } catch (error) {
    console.error('Error fetching profile:', error);
    profile.value = null;
  }
};

onMounted(fetchProfile);

const editProfile = () => {
  console.log('Edit profile clicked');
};
</script>

<style scoped>
body {
  font-family: 'Arial', sans-serif;
}
</style>
