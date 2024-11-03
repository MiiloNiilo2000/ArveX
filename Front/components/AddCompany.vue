<template>
    <div class="flex justify-center items-center pt-9">
      <div class="w-full max-w-4xl p-6 flex flex-col h-auto h-auto">
        <h2 class="text-2xl font-bold mb-4 text-center">Lisa ettevõte</h2>
        <UForm
          :validate="validate"
          :state="state"
          class="grid grid-cols-2 gap-4"
          @submit="onSubmit"
          @error="onError"
        >
          <UFormGroup label="Firma nimi" name="name">
            <UInput v-model="state.name" />
          </UFormGroup>
          <UFormGroup label="E-post" name="email">
            <UInput v-model="state.email" type="email" />
          </UFormGroup>
          <UFormGroup label="Registrikood" name="registerCode">
            <UTextarea v-model="state.registerCode" />
          </UFormGroup>
          <UFormGroup label="Käibemaksukohuslase number" name="vatNumber">
            <UInput v-model="state.vatNumber" />
          </UFormGroup>
          <UFormGroup label="Aadress" name="address">
            <UInput v-model="state.address" />
          </UFormGroup>
          <UFormGroup label="Postiindex" name="postalCode">
            <UInput v-model="state.postalCode" />
          </UFormGroup>
          <UFormGroup label="Riik" name="country">
            <UInput v-model="state.country" />
          </UFormGroup>
          <UFormGroup label="Pildi link">
            <UInput v-model="state.image" />
          </UFormGroup>
  
          <div class="col-span-2 flex justify-center">
            <UButton type="submit">Lisa</UButton>
            </div>
        </UForm>
      </div>
    </div>
  </template>
  <script setup lang="ts">
  import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
  import type { Company } from "../types/company";
  import { useCompanyStore } from '../stores/companyStores';
  import { reactive } from "vue";
  
  const companyStore = useCompanyStore();
  
  const state = reactive<Company>({
    id: 0,
    name: '',
    registerCode: 0,
    vatNumber: '',
    address: '',
    postalCode: 0,
    country: '',
    email: '',
    image: ''
  });
  
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
    companyStore.addCompany({ ...state });
    await navigateTo("/profiles");
  }
  
  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }
  </script>