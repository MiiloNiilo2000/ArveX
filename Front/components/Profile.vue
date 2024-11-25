<template>
  <div class="relative max-w-screen-2xl p-4">
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
      <!-- Kasutaja profiil -->
      <div class="bg-white shadow-md rounded-md p-6">
        <UserProfile :profile="profile" :editProfile="editProfile" />
      </div>

      <!-- Ettevõtte profiil või sõnum, kui ettevõtteid pole -->
      <div class="bg-white shadow-md rounded-md p-1">
        <div v-if="companies.length > 0">
          <!-- Ettevõtte valimine, kui ettevõtteid on -->
          <div v-if="companies.length > 1" class="mb-4">
            <label for="companySelect" class="block text-sm font-medium">
              Vali ettevõte:
            </label>
            <select
              v-model="selectedCompany"
              @change="onCompanyChange"
              id="companySelect"
              class="mt-1 block w-full border-emerald rounded-md shadow-sm"
            >
              <option v-for="company in companies" :key="company.companyId" :value="company">
                {{ company.name }}
              </option>
            </select>
          </div>

          <div v-if="selectedCompany" class="bg-white shadow-md rounded-md p-6">
            <CompanyProfile :company="selectedCompany" :editCompany="editCompany" />
          </div>
        </div>
        <!-- Kui ettevõtteid pole, kuvatakse tekst -->
        <div v-else class="text-center text-black py-12 text-xl font-bold">
          Siia ilmuvad teie ettevõtted
        </div>
      </div>

      <div class="bg-white shadow-md rounded-md p-6">
        <h3 class="text-lg font-bold mb-4">Lisa uus ettevõte</h3>
        <AddCompany @company-added="onCompanyAdded" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import UserProfile from '../components/UserProfile.vue';
import CompanyProfile from '../components/CompanyProfile.vue';
import AddCompany from '../components/AddCompany.vue';
import { useRouter } from 'vue-router';

const profile = ref<any>(null);
const companies = ref<any[]>([]);
const selectedCompany = ref<any>(null);
const router = useRouter();
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
    if (companies.value.length > 0) {
      selectedCompany.value = companies.value[0];
    }
  } catch (error) {
    console.error("Error fetching companies:", error);
  }
};

const onCompanyAdded = async () => {
  await fetchCompanies();
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

onMounted(() => {
  fetchLoggedInUserProfile();
});
</script>

<style scoped>

</style>
