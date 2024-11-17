<template>
  <div class="container">
    <h1 class="text-2xl font-bold mb-6">Tooted / teenused</h1>

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

    <div class="flex items-center mb-6">
      <UButton class="add-product-btn" icon="i-heroicons-plus"  @click="navigateToAddProduct">
      Lisa toode
    </UButton>
      <input
        v-model="searchTerm"
        type="text"
        placeholder="Otsi toodet..."
        class="form-search ml-6"
      />
    </div>

    

    <div v-for="(product, index) in filteredProducts" :key="index" class="bg-green-100 shadow-md rounded-lg p-3 w-1/3 mb-6">
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
const searchTerm = ref<string>('');

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
const filteredProducts = computed(() => {
  return products.value.filter(product =>
    product.name.toLowerCase().includes(searchTerm.value.toLowerCase()) ||
    product.description.toLowerCase().includes(searchTerm.value.toLowerCase())
  );
});

onMounted(async () => {
  await fetchCompanies();
  if (companies.value.length > 0) {
    selectedCompanyId.value = companies.value[0].companyId;
    fetchProducts();
  }
});
</script>

<style scoped>
.form-search {
  border: 2px solid #38a169; 
  background-color: #121212;
  color: rgb(255, 255, 255);
  border-radius: 0.375rem;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  width: auto;
  height: 2.5rem; 
  transition: border-color 0.2s ease-in-out;
  }

.form-search:focus {
  border-color: #38a169; 
  outline: none;
  box-shadow: 0 0 0 0.1rem #357955;
}

.add-product-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0.5rem 1rem;
  background-color: emerald;
  color: rgb(0, 0, 0);
  border: none;
  border-radius: 0.375rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
  font-size: 1rem;
}

.add-product-btn i {
  margin-right: 0.5rem; 
  font-size: 1.25rem; 
}

.add-product-btn:hover {
  background-color: #44b159; 
}

.add-product-btn:focus {
  outline: none;
  box-shadow: 0 0 0 0.1rem #357955; 
}
</style>