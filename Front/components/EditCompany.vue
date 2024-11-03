<template>
    <div class="flex justify-center items-center pt-9">
      <div class="w-96 p-6 flex flex-col h-99">
        <h2 class="text-2xl font-bold mb-4 text-center">Muuda ettevõtet</h2>
        <UForm
          :validate="validate"
          :state="state"
          class="space-y-4"
          @submit="onSubmit"
          @error="onError"
        >
          <UFormGroup label="Firma nimi" name="name">
            <UInput v-model="state.name" />
          </UFormGroup>
          <UFormGroup label="Registrikood" name="registerCode">
            <UInput v-model="state.registerCode" />
          </UFormGroup>
          <UFormGroup label="KMKN" name="vatNumber">
            <UInput v-model="state.vatNumber" />
          </UFormGroup>
          <UFormGroup label="Aadress" name="address">
            <UInput v-model="state.address" />
          </UFormGroup>
          <UFormGroup label="Postiindeks" name="postalCode">
            <UInput v-model="state.postalCode" />
          </UFormGroup>
          <UFormGroup label="Riik" name="country">
            <UInput v-model="state.country" />
          </UFormGroup>
          <UFormGroup label="Email" name="email">
            <UInput v-model="state.email" />
          </UFormGroup>
          <UButton type="submit"> Salvesta </UButton>
        </UForm>
      </div>
    </div>
  </template>

<script setup lang="ts">
  import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
  import type { Company } from "../types/company";
  import { reactive, onMounted } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import axios from "axios";

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

  const editCompany = async (company: Company) => {
    console.log("Attempting to update company:", company);
    try {
        console.log("Updating company:", company);
        
        const response = await axios.put(`http://localhost:5176/Companies/${companyId}`, company);

        console.log("Company updated successfully:", response.data);
        await router.push("/profiles");
    } catch (error) {
        console.error("Error updating company:", error);

        if (error.response) {
            console.error("Response data:", error.response.data);
            console.error("Response status:", error.response.status);
            console.error("Response headers:", error.response.headers);
        } else {
            console.error("Error message:", error.message);
        }
    }
};

  const route = useRoute();
  const router = useRouter();
  const companyId = Number(route.params.id);

  const fetchCompany = async (id: number) => {
    try {
        const response = await axios.get(`http://localhost:5176/Companies/${id}`);
        const company = response.data;
        if (company) {
            Object.assign(state, company);
        }
    } catch (error) {
        console.error("Error fetching company:", error);
    }
  };

  onMounted(async () => {
    await fetchCompany(companyId);
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
    console.log("Submitting company:", { ...state });
    await editCompany({ ...state });
  }

  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }

</script>