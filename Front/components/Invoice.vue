<template>
  <UForm
    :validate="validate"
    :state="state"
    class="space-y-4"
    @submit.prevent="submitForm"
    @error="onError"
  >
    <div class="flex w-full gap-20"> 
      <div class="w-1/2"> 

        <UFormGroup label="Firma nimi" name="title">
          <UInput
          v-model="state.title"
          @input="fetchCompanyNames"
          class="w-full h-12"
          color="emerald"
          placeholder="Sisesta firma nimi"
          list="company-suggestions"
        />
        <datalist id="company-suggestions">
          <option v-for="company in companySuggestions" :key="company.company_id" :value="company.name">
            {{ company.name }}
          </option>
        </datalist>
        </UFormGroup>

        <UFormGroup label="Aadress" name="address">
          <UInput v-model="state.address" class="w-full h-12" color="emerald" placeholder="'Ehitajate Tee 5'" />
        </UFormGroup>

        <UFormGroup label="Postiindeks" name="zipCode">
          <UInput v-model="state.zipCode" class="w-full h-12" color="emerald" placeholder="'12345'"/>
        </UFormGroup>

        <UFormGroup label="Riik" name="country">
          <UInput v-model="state.country" class="w-full h-12" color="emerald" placeholder="'Eesti'"/>
        </UFormGroup>
      </div>

      <div class="w-1/3"> 
        <UFormGroup label="Arve Number" name="invoiceNr">
          <UInput v-model="state.invoiceNumber" class="w-full h-12" color="emerald" placeholder="'54321'"/>
        </UFormGroup>

        <UFormGroup label="Kuupäev" name="dateCreated">
          <UInput 
            v-model="state.dateCreated" 
            type="date" 
            class="w-full h-12"
            color="emerald"
          />
        </UFormGroup>

        <UFormGroup label="Maksetähtaeg" name="dateDue">
          <UInput 
            v-model="state.dateDue" 
            type="date" 
            class="w-full h-12"
            color="emerald"
          />
        </UFormGroup>

        <UFormGroup label="Tingimused" name="condition">
          <UInput v-model="state.condition" class="w-full h-12" color="emerald" placeholder="'12 kuud'"/>
        </UFormGroup>

        <UFormGroup label="Viivis" name="delayFine">
          <UInput v-model="state.delayFine" class="w-full h-12" color="emerald" placeholder="'5% päevas'"/>
        </UFormGroup>

        <UFormGroup label="Font" name="font">
          <select v-model="state.selectedFont">
            <option v-for="font in fonts" :key="font" :value="font">
              {{ font }}
            </option>
          </select>
        </UFormGroup>

        <UFormGroup label="Vali Tooted" name="products">
          <div class="product-selection">
            <label v-for="product in availableProducts" :key="product.productId" class="product-item flex items-center">
              <input 
                type="checkbox" 
                :value="product" 
                v-model="selectedProducts" 
                class="mr-2" 
              />
              {{ product.name }} - {{ product.price }}
            </label>
          </div>
        </UFormGroup>
        
      </div>

      <div class="w-full">
        <h1 class="text-2xl font-bold">{{ 'Arve eelvaade' }}</h1>
          <div class="invoice-preview mt-10 p-6 bg-gray-100 shadow-md border rounded-lg">
            <div class="invoice-header text-center mb-6">
              <h1 class="text-2xl font-bold text-black">{{ state.title || 'Tallinn University of Technology' }}</h1>
            </div>

            <div class="flex justify-between">
              <div class="client-details w-6/12">
                <h2 class="text-lg font-semibold text-black">Klient:</h2>
                <h2 class="text-lg font-semibold text-black">{{ state.title || 'Tallinn University of Technology' }}</h2>
                <p class="text-black">
                  {{ state.address || 'Pärnu mnt 62/1, Kesklinna linnaosa' }}<br>
                  {{ state.zipCode || '10135' }}<br>
                  {{ state.country || 'Estonia' }}
                </p>
              </div>

            <div class="invoice-details w-6/12 text-right">
              
              <div class="flex justify-between mb-2">
                <div class="w-1/2 text-left">
                  <h3 class="text-lg font-semibold text-black">Arve number:</h3>
                  <p class="text-black"><span class="label">Kuupäev:</span></p>
                  <p class="text-black"><span class="label">Tingimused:</span></p>
                  <p class="text-black"><span class="label">Maksetähtaeg:</span></p>
                  <p class="text-black"><span class="label">Viivis:</span></p>
                </div>
                <div class="w-1/2 text-left">
                  <p class="text-lg font-semibold text-black"><span>{{ state.invoiceNumber  || '' }}</span></p>
                  <p class="text-black"><span>{{ state.dateCreated || '' }}</span></p>
                  <p class="text-black"><span>{{ state.condition || '' }}</span></p>
                  <p class="text-black"><span>{{ state.dateDue || '' }}</span></p>
                  <p class="text-black"><span>{{ state.delayFine || '' }}</span></p>
                </div>
              </div>
            </div>
            </div>
          </div>
      </div>
    </div>

    <UButton type="submit">Generate PDF</UButton>
  </UForm>
</template>

<script setup lang="ts">
  import { reactive, ref, watch, defineExpose, onMounted } from 'vue';
  import type { FormError, FormErrorEvent } from "#ui/types";
  import axios from 'axios';
  import { generateInvoicePDF } from '../stores/invoiceUtils';

  interface Company {
    company_id: string;
    reg_code: string;
    name: string;
    legal_address: string;
    zip_code: string;    
  }

  interface Product {
    productId: number;
    name: string;
    price: number;
  }

  const state = reactive({
    title: '',
    address: '',
    zipCode: '',
    country: 'Eesti',
    invoiceNumber: '',
    dateCreated: '',
    dateDue: '',
    condition: '',
    delayFine: '',
    selectedFont: 'Arial',
    footerImage: null,
    productIds: [],
  });

  const availableProducts = ref<Product[]>([]);
  const selectedProducts = ref<Product[]>([]);

  onMounted(async () => {
    try {
      const response = await axios.get('http://localhost:5176/Products/all')
      availableProducts.value = response.data;
    } catch (error) {
      console.error('Error fetching products:', error);
    }
  });

  const fonts = [
    'Times New Roman',
    'Arial',
    'Courier New',
    'Georgia',
    'Verdana',
  ]

  const validate = (state: any): FormError[] => {
    const errors = [];
    const zipString = state.zipCode.toString();
    if (!state.title) errors.push({ path: "title", message: "Required" });
    if (!state.address) errors.push({ path: "address", message: "Required" });
    if (!state.zipCode) errors.push({ path: "zipCode", message: "Required" });
    if (zipString.length < 5 || zipString.length > 5) errors.push({ path: "zipCode", message: "Postiindeks peab olema 5-kohaline number" });
    if (!state.invoiceNumber) errors.push({ path: "invoiceNr", message: "Required" });
    if (!state.dateCreated) errors.push({ path: "dateCreated", message: "Required" });
    if (!state.dateDue) errors.push({ path: "dateDue", message: "Required" });

    return errors;
  };
  
  const companySuggestions = ref<Company[]>([])

  const fetchCompanyNames = async () => {
      if (state.title.length < 3) return; 
    
      try {
        const response = await axios.get(`https://ariregister.rik.ee/est/api/autocomplete?q=${state.title}`);
        companySuggestions.value = response.data.data;
      } catch (error) {
        console.error('Error fetching company names:', error);
      }
    };

    watch(() => state.title, (newTitle) => {
      const selectedCompany = companySuggestions.value.find(company => company.name === newTitle);
      if (selectedCompany) {
        state.address = selectedCompany.legal_address;
        state.zipCode = selectedCompany.zip_code;
      }
    });
       
  const submitForm = () => { 
    state.productIds = selectedProducts.value.map(product => product.productId);
    generateInvoicePDF(state)
  };

  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }

  defineExpose({ validate, fetchCompanyNames, submitForm });
</script>

<style scoped>
  .invoice-preview {
    max-width: 600px;
    background: #f9f9f9;
    padding: 20px;
    border: 1px solid #ccc;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  }
  .invoice-header h1 {
    font-size: 24px;
  }
  .client-details, .invoice-details {
    margin-bottom: 20px;
  }
  .client-details h2, .invoice-details h3 {
    font-size: 18px;
    margin-bottom: 10px;
  }
  p {
    margin: 5px 0;
  }
  .label {
    font-weight: bold;
    margin-right: 10px;
  }
  .flex {
    display: flex;
  }
  .mb-2 {
    margin-bottom: 8px;
  }

  select {
  border: 1.5px solid #38a169; 
  background-color: black; 
  color: white; 
  border-radius: 0.375rem; 
  padding: 0.375rem 0.75rem; 
  font-size: 1rem; 
  transition: border-color 0.2s ease-in-out; 
  }

  select:focus {
    border-color: #2f855a; 
    outline: none; 
    box-shadow: 0 0 0 0.2rem rgba(56, 189, 248, 0.25); 
  }
  
  .form-field {
  border: 1px solid #38a169; 
  background-color: white;
  color: black;
  border-radius: 0.375rem;
  padding: 0.375rem 0.75rem;
  font-size: 1rem;
  width: 100%;
  height: 3rem; 
  transition: border-color 0.2s ease-in-out;
  }

  .form-field:focus {
    border-color: #2f855a; 
    outline: none;
    box-shadow: 0 0 0 0.2rem rgba(56, 189, 248, 0.25);
  }
</style>
