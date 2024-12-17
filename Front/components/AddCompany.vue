<template>
  <div class="flex justify-center items-center pt-9">
    <div class="w-full max-w-4xl p-6 flex flex-col h-auto bg-transparent">
      <h2 class="text-3xl font-bold mb-6 text-center">Lisa ettevõte</h2>
      <UForm
        :validate="validate"
        :state="state"
        class="grid grid-cols-2 gap-4"
        @submit="onSubmit"
        @error="onError"
      >
      <UFormGroup label="Firma nimi" name="name">
      <div class="relative">
        <UInput 
          v-model="state.name" 
          color="emerald" 
          class="bg-gray-900 rounded-md" 
          @input="fetchCompanyNames"
          list="company-suggestions"
           />
           <datalist id="company-suggestions">
          <option
            v-for="suggestion in companySuggestions"
            :key="suggestion.company_id"
            :value="suggestion.name"
          />
        </datalist>
      </div>
</UFormGroup>
        <UFormGroup label="E-post" name="email">
          <UInput v-model="state.email" type="email" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Registrikood" name="registerCode">
          <UInput v-model="state.registerCode" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Käibemaksukohuslase number" name="vatNumber">
          <UInput v-model="state.vatNumber" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Aadress" name="address">
          <UInput v-model="state.address" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Postiindex" name="postalCode">
          <UInput v-model="state.postalCode" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Riik" name="country">
          <UInput v-model="state.country" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Pank" name="bank">
          <UInput v-model="state.bank" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="IBAN" name="iban">
          <UInput v-model="state.iban" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="SWIFT" name="swift">
          <UInput v-model="state.swift" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>
        <UFormGroup label="Pildi link">
          <UInput v-model="state.image" color="emerald" class="bg-gray-900 rounded-md"/>
        </UFormGroup>

        <div class="col-span-2 flex justify-center">
          <UButton type="submit" class="add-btn w-1/3" icon="i-heroicons-plus">Lisa</UButton>
        </div>
      </UForm>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { Company } from "../types/company";
import { reactive } from "vue";
import { useRouter } from "vue-router";
import { useApi } from '../composables/useApi';
import { useApiForRik } from '../composables/useApiForRik';

const router = useRouter();
const { customFetch } = useApi();
const emit = defineEmits(['company-added']);
const companySuggestions = ref<CompanySuggestion[]>([]);
const { customFetchForRik } = useApiForRik();

interface CompanyForRik {
    name: string,
    registerCode: string,
    vatNumber: string,
    address: string,
    postalCode: string,
    country: string,
    email: string,
    bank: string,
    iban: string,
    swift: string,
    image: string,
  }

const state = reactive<CompanyForRik>({
  name: '',
  registerCode: null,
  vatNumber: '',
  address: '',
  postalCode: null,
  country: '',
  email: '',
  bank: "",
  iban: "",
  swift:"",
  image: '',
  companyId: 0
});


interface CompanySuggestion {
    company_id: string;
    reg_code: string;
    name: string;
    legal_address: string;
    zip_code: string;    
  }

const fetchCompanyNames = async () => {
    if (state.name.length < 3) return; 

    try {
      const response = await customFetchForRik<any>(`https://ariregister.rik.ee/est/api/autocomplete?q=${state.name}`, { method: 'GET' })
      companySuggestions.value = response.data;
    } catch (error) {
      console.error('Error fetching company names:', error);
    }
  };

watch(() => state.name, (newTitle) => {
  const selectedCompany = companySuggestions.value.find(company => company.name === newTitle);
  if (selectedCompany) {
    state.registerCode = selectedCompany.reg_code;
    state.address = selectedCompany.legal_address;
    state.postalCode = selectedCompany.zip_code;
  }
});

const addCompany = async (company: Omit<Company, 'CompanyId' | 'ProfileId'>) => {
  try {
    await customFetch('Companies', {
      method: 'POST',
      body: company
    });
    console.log("Ettevõte lisatud edukalt!");
    emit('company-added');
  } catch (error: any) {
    console.error("Server tagastas vea:", error?.data || error);
    alert("Validaatori vead: " + JSON.stringify(error?.data?.errors));
  }
};

const validate = (state: any): FormError[] => {
  const errors = [];
  if (!state.name) errors.push({ path: "name", message: "Required" });
  if (!state.email) errors.push({ path: "email", message: "Required"}) ;
  if (!state.registerCode) errors.push({ path: "registerCode", message: "Required" });
  if (!state.vatNumber) errors.push({ path: "vatNumber", message: "Required" });
  if (!state.address) errors.push({ path: "address", message: "Required" });
  if (!state.postalCode) errors.push({ path: "postalCode", message: "Required" });
  if (!state.country) errors.push({ path: "country", message: "Required" });
  if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(state.email)) {
    errors.push({ path: "email", message: "Invalid email format" });
  }
  return errors;
};

async function onSubmit(event: FormSubmitEvent<any>) {
  try {
    await addCompany({ ...state });
    alert("Ettevõte lisatud edukalt!");
    await router.push("/profiles");
  } catch (error) {
    console.error("Ettevõtte lisamisel tekkis viga:", error);
  }
}

async function onError(event: FormErrorEvent) {
  const element = document.getElementById(event.errors[0].id);
  element?.focus();
  element?.scrollIntoView({ behavior: "smooth", block: "center" });
}
</script>

<style>
@import '../assetsFront/styles/main.css';
</style>
