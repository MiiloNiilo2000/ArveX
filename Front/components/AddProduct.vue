<template>
  <div class="flex justify-center items-center">
      <div class="w-96 flex flex-col">
          <h2 class="text-3xl font-bold mb-6 text-center">Lisa toode</h2>
          
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4 "
      @submit="onSubmit"
      @error="onError"
    >
    <UFormGroup label="Toote nimi" name="name">
      <UInput v-model="state.name" color="emerald" class="bg-gray-900 rounded-md"/>
    </UFormGroup>

    <UFormGroup label="Kirjeldus" name="description">
      <UInput v-model="state.description" color="emerald" class="bg-gray-900 rounded-md"/>
    </UFormGroup>
    
    <UFormGroup label="Hind" name="price">
      <UInput v-model="state.price" color="emerald" class="bg-gray-900 rounded-md"/>
    </UFormGroup>

    <UFormGroup label="Maksuprotsent" name="taxPercent">
      <UInput v-model="state.taxPercent" color="emerald" class="bg-gray-900 rounded-md"/>
    </UFormGroup>

      <div class="col-span-2 flex justify-center">
      <UButton type="submit"> Lisa </UButton>

      </div>
    </UForm>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { Product } from "../types/product";
import { reactive, ref, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useApi } from '../composables/useApi';

const route = useRoute();
const router = useRouter();
const { customFetch } = useApi();
const companies = ref<{ companyId: number; name: string }[]>([]);
const emit = defineEmits(['product-added']);
const props = defineProps({
  selectedCompanyId: {
    type: Number,
    required: true
  }
});


const state = reactive<Product>({
    productId: 0,
    name: '',
    description: '',
    price: null,
    companyId: 0,
    taxPercent: null
  });

const addProduct = async (product: Product) => {
  try {
    await customFetch('Products', {
      method: 'POST',
      body: product,     
    });
    emit('product-added');
  } catch (error) {
    console.error("Error adding company:", error);
  }
};

const fetchCompanies = async () => {
    try {
      const response = await customFetch<Company[]>(`Profile/Companies`, { method: 'GET' });
      companies.value = response;
    } catch (error) {
      console.error("Error fetching companies:", error);
    }
};

const validate = (state: any): FormError[] => {
  const errors = [];
  if (!state.name) errors.push({ path: "name", message: "Required" });
  if (!state.description) errors.push({ path: "description", message: "Required" });
  if (!state.price) errors.push({ path: "price", message: "Required" });
  if (!state.taxPercent) errors.push({ path: "taxPercent", message: "Required" });
  return errors;
};
function resetForm() {
  state.name = '';
  state.description = '';
  state.price = null;
  state.taxPercent = null;
}

async function onSubmit(event: FormSubmitEvent<any>) {
  state.companyId = props.selectedCompanyId;
  await addProduct({ ...state });
  resetForm();
}

async function onError(event: FormErrorEvent) {
  const element = document.getElementById(event.errors[0].id);
  element?.focus();
  element?.scrollIntoView({ behavior: "smooth", block: "center" });
}

watch(() => props.selectedCompanyId, (newVal) => {
  state.companyId = newVal;
});

onMounted(() => {
  state.companyId = props.selectedCompanyId;
});
</script>

<style>
  @import '../assetsFront/styles/main.css';
</style>