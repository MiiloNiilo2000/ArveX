<template>
  <div class="container">
    
    <h1 class="text-2xl font-bold mb-6">Tooted / Teenused</h1>

    <div class="mb-6">
    <label for="companySelect" class="block text-sm font-medium">Vali ettevõte:</label>
    <div class="mt-1 w-1/3">
      <USelect 
        v-model="selectedCompanyId" 
        :options="companyOptions" 
        @change="onCompanyChange"
      />
    </div>
  </div>

    <UButton class="add-product-btn mb-6" icon="i-heroicons-plus"  @click="navigateToAddProduct">
      Lisa toode
    </UButton>

    <div v-for="(product, index) in products" :key="index" class="bg-green-100 shadow-md rounded-lg p-3 w-1/3 mb-6">
      <h2 class="text-black text-xl font-semibold">{{ product.name }}</h2>
      <p class="text-gray-600">{{ product.description }}</p>
      <p class="text-gray-600">Hind: {{ product.price }}€</p>
      <p class="text-gray-600">Maksuprotsent: {{ product.taxPercent }}%</p>
      <p class="text-gray-600">{{ getCompanyNameById(product.companyId) }}</p>

      <div class="flex justify-between mt-4">
        <UButton @click="navigateToEditProduct(product.productId)" title="Muuda"><i class="edit-icon">✏️</i></UButton>
        <UButton @click="deleteProduct(product.productId)" title="Kustuta"><Icon name="mdi-light:delete"/></UButton>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { onMounted, ref } from 'vue';
import { useApi } from '../composables/useApi';
import type { Product } from "../types/product";
import type { Company } from "../types/company";
import { useProductStore } from '../stores/productStores';

const router = useRouter();
const products = ref<Product[]>([]);
const companies = ref<Company[]>([]);
const selectedCompanyId = ref<number>();
const { customFetch } = useApi();
const { navigateToAddProduct, navigateToEditProduct } = useProductStore();

const companyOptions = computed(() => {
  return companies.value.map(company => ({
    label: company.name,
    value: company.companyId,
  }));
});

const fetchProducts = async () => {
  if (selectedCompanyId.value) {
    try {
      const response = await customFetch<Product[]>(`Companies/${selectedCompanyId.value}/products`, { method: 'GET' });
      products.value = response;
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  }
};

const fetchCompanies = async () => {
  try {
    const response = await customFetch<Company[]>(`Companies/all`, { method: 'GET' });
    companies.value = response;
  } catch (error) {
    console.error("Error fetching companies:", error);
  }
};

const onCompanyChange = () => {
  fetchProducts();
};

const deleteProduct = async (id: number) => {
  try {
    await customFetch<Product[]>(`Products/${id}`, { method: 'DELETE' });
    products.value = products.value.filter(product => product.productId !== id);
    fetchProducts();
  } catch (error) {
    console.error("Error deleting product:", error);
  }
};
const getCompanyNameById = (companyId: number) => {
  const company = companies.value.find(c => c.companyId === companyId);
  return company ? company.name : 'Ettevõte ei leitud';
};

onMounted(async () => {
  await fetchCompanies();
  if (companies.value.length > 0) {
    selectedCompanyId.value = companies.value[0].companyId;
    fetchProducts();
  }
});
</script>

<style scoped>
/* Override the background color of the dropdown options */
.custom-select .u-dropdown-menu {
  background-color: #00ff4c !important; /* Change background color */
  border-radius: 8px !important; /* Optional: Adjust border radius */
  color: #ff0000 !important; /* Optional: Change text color */
}

/* Optional: Adjust the color of each dropdown item */
.custom-select .u-dropdown-menu .u-dropdown-item {
  color: #f90101d2 !important;
}

/* Optional: Hover effect on dropdown items */
.custom-select .u-dropdown-menu .u-dropdown-item:hover {
  background-color: #a7f3d0 !important; /* Hover background color */
}
</style>