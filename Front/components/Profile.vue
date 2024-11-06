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

    <div class="flex space-x-6 mt-6">
      <div class="flex-1">
        <label for="userSelect" class="block text-sm font-medium text-gray-700">Vali kasutaja:</label>
        <select v-model="selectedProfileId" id="userSelect" @change="onProfileChange" class="mt-1 block w-full border-gray-300 rounded-md shadow-sm">
          <option v-for="profile in profiles" :key="profile.profileId" :value="profile.profileId">
            {{ profile.username }}
          </option>
        </select>
      </div>

      <div class="flex-1">
        <label for="companySelect" class="block text-sm font-medium text-gray-700">
          Vali ettevõte:
        </label>
        <select v-model="selectedCompanyId" id="companySelect" @change="onCompanyChange" class="mt-1 block w-full border-gray-300 rounded-md shadow-sm">
          <option v-for="company in companies" :key="company.companyId" :value="company.companyId">
            {{ company.name }}
          </option>
        </select>
      </div>
    </div>

    <div class="flex space-x-6 mt-6">
      <div v-if="selectedProfile" class="flex-1 mr-8">
        <UserProfile :profile="selectedProfile" :editProfile="editProfile" />
      </div>

      <div v-if="selectedCompany" class="flex-1 mr-8">
        <CompanyProfile :company="selectedCompany" :editCompany="editCompany" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import UserProfile from '../components/UserProfile.vue';
import CompanyProfile from '../components/CompanyProfile.vue';
import axios from 'axios';

const router = useRouter();

const profiles = ref([]);
const selectedProfileId = ref<number | null>(null);
const selectedProfile = computed(() => {
  return profiles.value.find(user => user.profileId === selectedProfileId.value);
});

const companies = ref([]);
const selectedCompanyId = ref<number | null>(null);
  const selectedCompany = computed(() => {
  return companies.value.find(company => company.companyId === selectedCompanyId.value);
});

const fetchCompanies = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Companies/all');
    companies.value = response.data;
    console.log("Fetched companies:", companies.value);
  } catch (error) {
    console.error("Error fetching companies:", error);
  }
};

const fetchProfiles = async () => {
  try {
    const response = await axios.get('http://localhost:5176/Profile/all');
    profiles.value = response.data;
    console.log("Fetched profiles:", profiles.value);
  } catch (error) {
    console.error("Error fetching profiles:", error);
  }
};

const fetchCompaniesForProfile = async (profileId: number) => {
  try {
    const response = await axios.get(`http://localhost:5176/Profile/${profileId}/companies`);
    companies.value = response.data;
    console.log("Fetched companies for profile:", profileId, companies.value);
  } catch (error) {
    console.error("Error fetching companies for profile:", error);
  }
};

const onProfileChange = async () => {
  if (selectedProfileId.value !== null) {
    await fetchCompaniesForProfile(selectedProfileId.value);
    selectedCompanyId.value = null;
  }
};

const onCompanyChange = async () => {
  console.log('Selected company ID:', selectedCompanyId.value);
  console.log('Selected company:', selectedCompany.value);
};

onMounted(async () => {
  await fetchProfiles();
  if (profiles.value.length > 0) {
    selectedProfileId.value = profiles.value[0].profileId;
    await fetchCompaniesForProfile(selectedProfileId.value);
  }
});

const editProfile = () => {
  alert('Profile editing not implemented yet!');
};

const editCompany = () => {
  router.push(`/companies/edit/${selectedCompanyId.value}`);
};

const addUser = () => {
  router.push('/add-profile');
};

const addCompany = () => {
  router.push('/add-company');
};
</script>

<style scoped>
body {
  background-color: #f3f4f6;
}
</style>