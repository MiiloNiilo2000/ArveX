<template>
  <div class="relative max-w-screen-xl p-8">
    <div class="flex space-x-4 mb-6">
      <button @click="addUser" class="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600">
        Lisa kasutaja
      </button>
      <button @click="addCompany" class="bg-green-500 text-white px-4 py-2 rounded-md hover:bg-green-600">
        Lisa ettevõte
      </button>
    </div>

    <div v-if="profile" class="flex space-x-8">
      <div class="flex-1">
        <UserProfile :profile="profile" :editProfile="editProfile" />
      </div>

      <div class="flex-1">
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

        <div v-if="selectedCompany">
          <CompanyProfile :company="selectedCompany" :editCompany="editCompany" />
        </div>
      </div>
    </div>

    <div v-else>
      <p>Sa pole veel profiili loonud.</p>
    </div>

    <div v-if="profile && !companies.length">
      <p>Sul pole veel ühtegi ettevõtet lisatud.</p>
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

const fetchLoggedInUserProfile = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Profile/Me', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });

    if (response.data) {
      profile.value = response.data;
      await fetchUserCompanies(); 
    }
  } catch (error) {
    console.error("Error fetching logged-in user profile:", error);
  }
};

const fetchUserCompanies = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Profile/Companies', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });

    if (response.data && response.data.length > 0) {
      companies.value = response.data;
      selectedCompany.value = companies.value[0];
    }
  } catch (error) {
    console.error("Error fetching user companies:", error);
  }
};

const editProfile = () => {
  alert('Profile editing not implemented yet!');
};

const editCompany = () => {
  alert('Company editing not implemented yet!');
};

const onCompanyChange = () => {
  console.log('Selected company:', selectedCompany.value);
};

const addUser = () => {
  router.push('/add-profile');
};

const addCompany = () => {
  router.push('/add-company');
};

onMounted(() => {
  fetchLoggedInUserProfile();
});
</script>

<style scoped>
body {
  background-color: #f3f4f6;
}
</style>
