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
        <select v-model="selectedUserId" id="userSelect" class="mt-1 block w-full border-gray-300 rounded-md shadow-sm">
          <option v-for="user in users" :key="user.id" :value="user.id">
            {{ user.username }}
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
      <div v-if="selectedUser" class="flex-1 mr-8">
        <UserProfile :profile="selectedUser" :editProfile="editProfile" />
      </div>

      <div v-if="selectedCompany" class="flex-1 mr-8">
        <CompanyProfile :company="selectedCompany" :editCompany="editCompany" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useProfileStore } from '../stores/profileStores';
import { useRouter } from 'vue-router';
import UserProfile from '../components/UserProfile.vue';
import CompanyProfile from '../components/CompanyProfile.vue';
import axios from 'axios';

const profileStore = useProfileStore();
const router = useRouter();

const users = computed(() => profileStore.profiles);
const selectedUserId = ref<number | null>(users.value[0]?.id || null);
const selectedUser = computed(() => {
  return selectedUserId.value !== null
    ? users.value.find(user => user.id === selectedUserId.value) || null
    : null;
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

const onCompanyChange = async () => {
  console.log('Selected company ID:', selectedCompanyId.value);
console.log('Selected company:', selectedCompany.value);
};

onMounted(async () => {
  await fetchCompanies();
  if (companies.value.length > 0) {
      selectedCompanyId.value = 1;
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