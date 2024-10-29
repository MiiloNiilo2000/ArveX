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
        <label for="companySelect" class="block text-sm font-medium text-gray-700">Vali ettevõte:</label>
        <select v-model="selectedCompanyId" id="companySelect" class="mt-1 block w-full border-gray-300 rounded-md shadow-sm">
          <option v-for="company in companies" :key="company.id" :value="company.id">
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
import { computed, ref } from 'vue';
import { useProfileStore } from '../stores/profileStores';
import { useCompanyStore } from '../stores/companyStores';
import { useRouter } from 'vue-router';
import UserProfile from '../components/UserProfile.vue';
import CompanyProfile from '../components/CompanyProfile.vue';

const profileStore = useProfileStore();
const companyStore = useCompanyStore();
const router = useRouter();

// Loendid profiilide ja ettevõtete jaoks
const users = computed(() => profileStore.profiles);
const companies = computed(() => companyStore.companies);

// Valitud kasutaja ja ettevõtte ID
const selectedUserId = ref<number | null>(users.value[0]?.id || null);
const selectedCompanyId = ref<number | null>(companies.value[0]?.id || null);

const selectedUser = computed(() => {
  return selectedUserId.value !== null
    ? users.value.find(user => user.id === selectedUserId.value) || null
    : null;
});

const selectedCompany = computed(() => {
  return selectedCompanyId.value !== null
    ? companies.value.find(company => company.id === selectedCompanyId.value) || null
    : null;
});

const editProfile = () => {
  alert('Profile editing not implemented yet!');
};

const editCompany = () => {
  alert('Company editing not implemented yet!');
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