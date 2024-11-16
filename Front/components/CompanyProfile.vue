<template>
  <div class="max-w-full p-8">
    <h1 class="text-3xl font-bold mb-6">{{ title }}</h1>

    <div class="bg-green-100 shadow-md rounded-lg p-6 relative">
      <img
        v-if="company.image"
        :src="company.image"
        alt="Ettevõtte pilt"
        class="absolute top-4 right-4 rounded-full w-24 h-24 mr-6"
      />
      <h2 class="text-xl text-black font-semibold">{{ company.name }}</h2>
      <p class="text-gray-600">KMKN: {{ company.vatNumber }}</p>
      <p class="text-gray-600">Aadress: {{ company.address }}</p>
      <p class="text-gray-600">Riik: {{ company.country }}</p>
      <p class="text-gray-600">Registrikood: {{ company.registerCode }}</p>
      <p class="text-gray-600">Postiindeks: {{ company.postalCode }}</p>
      <p class="text-gray-600">E-mail: {{ company.email }}</p>

      <div class="mt-6">
        <UButton @click="editCompany" class="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600">
          Muuda ettevõtet
        </UButton>
      </div>
    </div>
  </div>
</template>
  
<script setup lang="ts">
  
  import { defineProps } from 'vue';

  const title = 'Ettevõtte profiil'
  
  const props = defineProps<{
    company: {
      name: string;
      vatNumber: string;
      address: string;
      postalCode: number;
      country: string;
      registerCode: number;
      email: string;
      image: string;
    };
    editCompany: () => void;
  }>();

</script>
