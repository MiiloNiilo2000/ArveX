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

      <!-- Ettevõtte info ja dropdown -->
      <div class="flex-1">
        <!-- Ettevõtte valiku dropdown -->
        <div v-if="companies.length > 1" class="mb-4">
          <label for="companySelect" class="block text-sm font-medium text-gray-700">
            Vali ettevõte:
          </label>
          <select v-model="selectedCompany" @change="onCompanyChange" id="companySelect" class="mt-1 block w-full border-gray-300 rounded-md shadow-sm">
            <option v-for="company in companies" :key="company.companyId" :value="company">
              {{ company.name }}
            </option>
          </select>
        </div>

        <!-- Ettevõtte info kuvamine -->
        <div v-if="selectedCompany">
          <CompanyProfile :company="selectedCompany" :editCompany="editCompany" />
        </div>
      </div>
    </div>

    <!-- Kui profiil puudub -->
    <div v-else>
      <p>Profiili andmeid ei leitud.</p>
    </div>

    <!-- Kui ettevõtet ei leitud -->
    <div v-if="profile && !companies.length">
      <p>Ettevõtte andmeid ei leitud.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import UserProfile from '../components/UserProfile.vue';
import CompanyProfile from '../components/CompanyProfile.vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const profile = ref<any>(null);
const companies = ref<any[]>([]);
const selectedCompany = ref<any>(null);
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
      await fetchUserCompanies();  // Kui profiil on saadud, lae ka ettevõtted
    }
  } catch (error) {
    console.error("Error fetching logged-in user profile:", error);
  }
};

// Funktsioon kasutaja seotud ettevõtete toomiseks
const fetchUserCompanies = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Profile/Companies', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });

    // Kui ettevõtted leiti, salvestame need
    if (response.data && response.data.length > 0) {
      companies.value = response.data;
      selectedCompany.value = companies.value[0]; // Eelmäärame esimese ettevõtte
    }
  } catch (error) {
    console.error("Error fetching user companies:", error);
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

// Funktsioon ettevõtte vahetamiseks
const onCompanyChange = () => {
  console.log('Selected company:', selectedCompany.value);
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
