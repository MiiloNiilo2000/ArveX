<template>
  <div class="justify-center">
    <div class="grid grid-cols-1 sm:grid-cols-2">
      <div class="p-6 ml-64 mr-12 mt-8">
        <h1 class="text-3xl font-bold mb-6 text-center">Tooted / Teenused</h1>

        <div class="mb-6 flex justify-center">
          <div class="w-1/3">
            <label for="companySelect" class="block text-sm font-medium text-center mb-1">Vali ettevõte:</label>
            <USelect 
              v-model="state.selectedCompanyId" 
              :options="companyOptions" 
              @change="onCompanyChange"
              class="border-2 border-green-600 rounded-md w-full"
            />
          </div>
        </div>

        <div class="mb-6 flex justify-center">
            <input
              v-model="searchTerm"
              type="text"
              placeholder="Otsi toodet..."
              class="form-search"
              color="emerald"
            />
        </div>

       <div v-if="state.products.length > 0">
        <div v-for="(product, index) in filteredProducts" :key="index" class="bg-green-100 shadow-md rounded-lg p-3 w-full mb-6">
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
       <div v-else class="text-center mt-6">
          <p>Valitud ettevõttel ei ole ühtegi toodet.</p>
      </div>
      </div>

      <div class="ml-12 mr-64 mt-14">
        <AddProduct :selected-company-id="state.selectedCompanyId" @product-added="onProductAdded"/>
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
import AddProduct from './AddProduct.vue';

const router = useRouter();
const { customFetch } = useApi();
const { navigateToEditProduct, state, getCompanyNameById} = useProductStore();
const searchTerm = ref<string>('');

const companyOptions = computed(() => {
  return state.companies.map(company => ({
    label: company.name,
    value: company.companyId,
  }));
});

const fetchProducts = async () => {
  state.products = [];
  if (state.selectedCompanyId) {
    try {
      const response = await customFetch<Product[]>(`Companies/${state.selectedCompanyId}/Products`, { method: 'GET' });
      state.products = response;
    } catch (error) {
      console.error("Error fetching companies:", error);
    }
  }
};

const fetchCompanies = async () => {
    try {
      const response = await customFetch<Company[]>(`Profile/Companies`, { method: 'GET' });
        state.companies = response;
    } catch (error) {
      console.error("Error fetching companies:", error);
    }
};

const onCompanyChange = () => {
  fetchProducts();
};
const onProductAdded = async () => {
  await fetchProducts();
};

const deleteProduct = async (id: number) => {
  const confirmed = window.confirm("Kas olete kindel, et soovite toote kustutada?");
  if (!confirmed) return;
  
  try {
    await customFetch<Product[]>(`Products/${id}`, { method: 'DELETE' });
    state.products = state.products.filter(product => product.productId !== id);
    fetchProducts();
  } catch (error) {
    console.error("Error deleting product:", error);
  }
};
const filteredProducts = computed(() => {
  return state.products.filter(product =>
    product.name.toLowerCase().includes(searchTerm.value.toLowerCase()) ||
    product.description.toLowerCase().includes(searchTerm.value.toLowerCase())
  );
});

onMounted(async () => {
  await fetchCompanies();
  if (state.selectedCompanyId) {
      fetchProducts();
    } else {
      state.selectedCompanyId = state.companies[0].companyId;
      fetchProducts();
    }
});
</script>

<style>
  @import '../assetsFront/styles/main.css';
</style>