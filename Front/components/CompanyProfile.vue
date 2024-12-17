<template>
  <div class="max-w-full p-8">
    <h1 class="text-3xl font-bold mb-6 text-center">{{ title }}</h1>

    <div class="bg-green-100 shadow-md rounded-lg p-6 relative">
      <img
        v-if="company.image"
        :src="company.image"
        alt="Ettevõtte pilt"
        class="absolute top-4 right-0 rounded-full w-24 h-24 mr-6"
      />
      <h2 class="text-xl text-black font-semibold">{{ company.name }}</h2>
      <p class="text-gray-600">KMKN: {{ company.vatNumber }}</p>
      <p class="text-gray-600">Riik: {{ company.country }}</p>
      <p class="text-gray-600">Registrikood: {{ company.registerCode }}</p>
      <p class="text-gray-600">Aadress: {{ company.address }}</p>
      <p class="text-gray-600">Postiindeks: {{ company.postalCode }}</p>
      <p class="text-gray-600">E-mail: {{ company.email }}</p>
      <p class="text-gray-600">Pank: {{ company.bank }}</p>
      <p class="text-gray-600">IBAN: {{ company.iban }}</p>
      <p class="text-gray-600">SWIFT: {{ company.swift }}</p>

      <div class="mt-6 grid grid-cols-1 grid-cols-2 gap-8">
        <UButton @click="editCompany" class="add-btn" icon="i-heroicons-pencil">
          Muuda
        </UButton>
        <UButton @click="deleteCompany(company.companyId)" class="add-btn" icon="i-heroicons-trash">
          Kustuta
        </UButton>
      </div>
    </div>
  </div>
</template>
  
<script setup lang="ts">
  
  import { defineProps } from 'vue';
  import type { Company } from "../types/company";
  import { useApi } from '../composables/useApi';

  const { customFetch } = useApi();
  const title = 'Ettevõtte profiil'
  const companies = ref<Company[]>([]);
  const emit = defineEmits(['company-deleted']);
  const router = useRouter();
  
  const props = defineProps<{
    company: {
      companyId: number;
      name: string;
      vatNumber: string;
      address: string;
      postalCode: number;
      country: string;
      registerCode: number;
      email: string;
      bank: string;
      iban: string;
      swift: string;
      image: string;
    };
  }>();

  const editCompany = () => {
  router.push(`/companies/edit/${props.company.companyId}`);
};
  const deleteCompany = async (id: number) => {
    const confirmed = window.confirm("Kas olete kindel, et soovite ettevõtte kustutada?");
    if (!confirmed) return;
  try {
    await customFetch<Company[]>(`Companies/${id}`, { method: 'DELETE' });
    companies.value = companies.value.filter(company => company.companyId !== id);
    emit('company-deleted');
  } catch (error) {
    console.error("Error deleting product:", error);
  }
};

</script>

<style>
  @import '../assetsFront/styles/main.css';
</style>
