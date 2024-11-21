<template>
  <div class="relative max-w-screen-xl p-8">
    <!-- Lisamise nupud -->
    <div class="flex space-x-4 mb-6">
      <button @click="addUser" class="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600">
        Lisa kasutaja
      </button>
      <button @click="addCompany" class="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600">
        Lisa ettevõte
      </button>
    </div>

    <!-- Kasutaja profiil -->
    <div v-if="profile" class="flex flex-col">
      <UserProfile :profile="profile" :editProfile="editProfile" />
    </div>
    
    <!-- Laadimise või veateade -->
    <div v-else>
      <p>Laadimine või viga profiili laadimisel...</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import UserProfile from '../components/UserProfile.vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const profile = ref<any>(null);
const router = useRouter();

// Funktsioon kasutaja profiili toomiseks
const fetchLoggedInUserProfile = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Profile/Me', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });

    profile.value = response.data;
  } catch (error) {
    console.error("Error fetching logged-in user profile:", error);
  }
};

// Funktsioon profiili redigeerimiseks
const editProfile = () => {
  alert('Profile editing not implemented yet!');
};

// Funktsioon kasutaja lisamiseks
const addUser = () => {
  router.push('/add-profile');
};

// Funktsioon ettevõtte lisamiseks
const addCompany = () => {
  router.push('/add-company');
};

onMounted(() => {
  // Kutsume välja profiili laadimise pärast komponendi mountimist
  fetchLoggedInUserProfile();
});
</script>

<style scoped>
body {
  background-color: #f3f4f6;
}
</style>
