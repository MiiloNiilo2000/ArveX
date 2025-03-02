<template>
  <div class="justify-center mt-8">
    <div v-if="companies.length > 0" class="grid grid-cols-1 sm:grid-cols-2">
      <div class="shadow-md p-1 ml-64 mr-12">
        <div v-if="companies.length > 1" class="w-full max-w-xs mx-auto">
          <label for="companySelect" class="text-sm font-medium">
            Vali ettevõte:
          </label>
          <select
            v-model="selectedCompany"
            @change="onCompanyChange"
            id="companySelect"
            class="mt-1 block w-full border-emerald rounded-md"
          >
            <option v-for="company in companies" :key="company.companyId" :value="company">
              {{ company.name }}
            </option>
          </select>
        </div>

        <div v-if="selectedCompany">
          <CompanyProfile
            :company="selectedCompany"
            @company-deleted="onCompanyAdded"
          />
        </div>
      </div>

      <div class="ml-12 mr-64 mt-10">
        <AddCompany @company-added="onCompanyAdded" />
      </div>
    </div>

    <div v-else class="flex justify-center items-center h-[50vh]">
      <div class="w-full max-w-2xl p-8 shadow-md bg-transparent rounded-lg">
        <AddCompany @company-added="onCompanyAdded" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, nextTick } from 'vue';
import CompanyProfile from '../components/CompanyProfile.vue';
import AddCompany from '../components/AddCompany.vue';
import { useApi } from '../composables/useApi';

const profile = ref<any>(null);
const companies = ref<any[]>([]);
const selectedCompany = ref<any>(null);
const { customFetch } = useApi();

const fetchLoggedInUserProfile = async () => {
  try {
    const response = await customFetch<Profile[]>(`Profile/Me`, { method: 'GET' });
    profile.value = response;
    await fetchCompanies();
  } catch (error) {
    console.error("Error fetching profile:", error);
  }
};

const fetchCompanies = async () => {
  try {
    const response = await customFetch<Company[]>(`Profile/Companies`, { method: 'GET' });
    companies.value = response;
    
    const savedCompanyId = localStorage.getItem('selectedCompanyId');
    if (savedCompanyId) {
      const savedCompany = companies.value.find(company => company.companyId === Number(savedCompanyId));
      if (savedCompany) {
        selectedCompany.value = savedCompany;
      }
    }

    if (!selectedCompany.value && companies.value.length > 0) {
      selectedCompany.value = companies.value[0];
    }

  } catch (error) {
    console.error("Error fetching companies:", error);
  }
};

const onCompanyAdded = async () => {
  await fetchCompanies();
};

const onCompanyChange = () => {
  console.log('Selected company:', selectedCompany.value);

  if (selectedCompany.value) {
    localStorage.setItem('selectedCompanyId', selectedCompany.value.companyId.toString());
  }
};

onMounted(() => {
  fetchLoggedInUserProfile();
});
</script>

<style scoped>
</style>
