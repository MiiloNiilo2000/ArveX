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

    <!-- Kasutaja profiil ja ettevõtte info -->
    <div v-if="profile" class="flex space-x-8">
      <!-- Kasutaja profiil -->
      <div class="flex-1">
        <UserProfile :profile="profile" :editProfile="editProfile" />
      </div>

      <!-- Ettevõtte info -->
      <div v-if="company" class="flex-1">
        <!-- Siin kasutame CompanyProfile komponenti -->
        <CompanyProfile :company="company" :editCompany="editCompany" />
      </div>
    </div>

    <!-- Kui profiil puudub -->
    <div v-else>
      <p>Profiili andmeid ei leitud.</p>
    </div>

    <!-- Kui ettevõtet ei leitud -->
    <div v-if="profile && !company">
      <p>Ettevõtte andmeid ei leitud.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import UserProfile from '../components/UserProfile.vue';
import CompanyProfile from '../components/CompanyProfile.vue'; // Lisa see komponent
import axios from 'axios';
import { useRouter } from 'vue-router';

const profile = ref<any>(null);
const company = ref<any>(null);
const router = useRouter();

// Funktsioon kasutaja profiili toomiseks
const fetchLoggedInUserProfile = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Profile/Me', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });

    // Kui profiil leiti, salvestame selle
    if (response.data) {
      profile.value = response.data;
      await fetchUserCompany();  // Kui profiil on saadud, lae ka ettevõtte info
    }
  } catch (error) {
    console.error("Error fetching logged-in user profile:", error);
  }
};

// Funktsioon kasutaja seotud ettevõtte toomiseks
const fetchUserCompany = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Profile/Companies', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });

    // Kui ettevõte leiti, salvestame selle
    if (response.data && response.data.length > 0) {
      company.value = response.data[0]; // Kui ettevõte on üks, siis võta esimene
    }
  } catch (error) {
    console.error("Error fetching user company:", error);
  }
};

// Funktsioon profiili redigeerimiseks
const editProfile = () => {
  alert('Profile editing not implemented yet!');
};

// Funktsioon ettevõtte redigeerimiseks
const editCompany = () => {
  alert('Company editing not implemented yet!');
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
