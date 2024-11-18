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
      <UDivider label="Kopeeri varasema arve andmed" class="h-10 mb-2" />
      <UFormGroup name="pastInvoice" class="flex justify-center h-16">
        <select v-model="state.pastInvoice">
          <option value="" disabled>Vali varasem arve:</option>
          <option v-for="invoice in pastInvoices" :key="invoice.invoiceId" :value="invoice.invoiceId">
            {{ formatInvoiceOption(invoice) }}
          </option>
        </select>
      </UFormGroup>
      <UDivider label="Loo uus arve" />
    </div>
  </div>

  <div class="flex w-full gap-20"> 

    <div class="w-1/4"> 

      <UFormGroup label="Firma nimi" name="title">
        <UInput
        v-model="state.title"
        @input="fetchCompanyNames"
        class="w-full bg-gray-900 rounded-md mb-4"
        color="emerald"
        placeholder="Sisesta firma nimi"
        list="company-suggestions"
      />
      <datalist id="company-suggestions">
        <option 
          v-for="company in companySuggestions" 
          :key="company.company_id" 
          :value="company.name">
          {{ company.name }}
        </option>
      </datalist>
      </UFormGroup>

      <UFormGroup label="Registrikood" name="clientRegNr">
        <UInput 
          v-model="state.clientRegNr" 
          class="w-full bg-gray-900 rounded-md mb-4" 
          color="emerald" 
          placeholder="'10379733'" />
      </UFormGroup>

      <UFormGroup label="Käibemaksukohustuslase number" name="clientKMKR">
        <UInput 
          v-model="state.clientKMKR" 
          class="w-full bg-gray-900 rounded-md mb-4" 
          color="emerald" 
          placeholder="'EE100247019'" />
      </UFormGroup>

      <UFormGroup label="Aadress" name="address">
        <UInput 
          v-model="state.address" 
          class="w-full bg-gray-900 rounded-md mb-4" 
          color="emerald" 
          placeholder="'Harju maakond, Tallinn, Nõmme linnaosa, Pärnu mnt 238'" />
      </UFormGroup>

      <UFormGroup label="Postiindeks" name="zipCode">
        <UInput 
          v-model="state.zipCode" 
          class="w-full bg-gray-900 rounded-md mb-4" 
          color="emerald" 
          placeholder="'12345'"/>
      </UFormGroup>

      <UFormGroup label="Riik" name="country">
        <UInput 
          v-model="state.country" 
          class="w-full bg-gray-900 rounded-md mb-4" 
          color="emerald" 
          placeholder="'Eesti'"/>
      </UFormGroup>

      
    </div>

    <div class="w-1/5"> 

      <UFormGroup label="Arve Number" name="invoiceNr">
        <UInput 
          v-model="state.invoiceNumber" 
          class="w-full bg-gray-900 rounded-md mb-4" 
          color="emerald" 
          placeholder="'54321'"/>
      </UFormGroup>


      <UFormGroup label="Kuupäev" name="dateCreated">
        <UInput 
          v-model="state.dateCreated" 
          type="date" 
          class="w-full bg-gray-900 rounded-md mb-4"
          color="emerald"
        />
      </UFormGroup>

      <UFormGroup label="Maksetähtaeg" name="dateDue">
        <UInput 
          v-model="state.dateDue" 
          type="date" 
          class="w-full bg-gray-900 rounded-md mb-4"
          color="emerald"
        />
      </UFormGroup>

      <UFormGroup label="Tingimused" name="condition">
        <UInput 
          v-model="state.condition" 
          class="w-full bg-gray-900 rounded-md mb-4" 
          color="emerald" 
          placeholder="'12 kuud'"/>
      </UFormGroup>

      <UFormGroup label="Viivis" name="delayFine">
        <UInput 
          v-model="state.delayFine" 
          class="w-full bg-gray-900 rounded-md mb-4" 
          color="emerald" 
          placeholder="'5% päevas'"/>
      </UFormGroup>  
    </div>

      <div class="w-100">
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

  <div class="w-1/2 h-auto">
  <UDivider label="Vali Tooted" class="h-10 mb-2" />
  <div class="flex items-center mb-4">
      <UButton @click="navigateToAddProduct" class="add-btn mr-4" icon="i-heroicons-plus">Lisa uus toode</UButton>
      <input
      v-model="searchTerm"
      type="text"
      placeholder="Otsi toodet..."
      class="form-search"
    />
  </div>

      <UFormGroup name="products" class="h-64 overflow-y-auto">
            <div class="product-selection">
              <div 
                v-for="product in filteredProducts" 
                :key="product.productId" 
                class="product-item flex items-center mb-4 border-t-2 border-b-2 border-emerald-800 rounded-b rounded-t pb-2 mr-2 ml-2">
                
                <input 
                  type="checkbox" 
                  :value="product" 
                  :checked="selectedProducts.some(p => p.productId === product.productId)"
                  v-model="selectedProducts" 
                  @change="toggleProductSelection(product.productId)" 
                  class="custom-checkbox mr-2 mt-2"
                />
                <span class="mt-2">{{ product.name }} - {{ product.price + "€" }}</span>

                <div v-if="state.productsAndQuantities[product.productId] !== undefined" class="flex items-center ml-auto">
                  <label class="text-sm font-medium text-gray-500 mt-1">
                    Kogus:
                  </label>

                  <UInput 
                  v-if="state.productsAndQuantities[product.productId] !== undefined" 
                  type="number" 
                  class="ml-2 w-16 mt-2" 
                  color="emerald"
                  v-model.number="state.productsAndQuantities[product.productId]" 
                  min="1" 
                  @input="updateQuantity(product.productId)"
                />
                  <div class="ml-6 mt-3">
                    <div>
                      <UDropdown :items="productDropdownItems(product.productId)" :popper="{ offsetDistance: 4, placement: 'right-start' }">
                        <UButton class="mr-2" size="md" color="emerald" variant="ghost" :padded="false" trailing-icon="i-heroicons-ellipsis-horizontal-circle" />
                      </UDropdown>
                    </div>
                  </div>
                </div>   
              </div>
            </div>
          </UFormGroup>
      </div>

    <div class="flex w-full gap-20"> 
      <div class="w-1/2"> 
        <UDivider label="Kujunda Arvet" class="h-10 mb-2" />

        <UFormGroup label="Font" name="font" class="h-20">
          <select v-model="state.selectedFont" class="font-selector" :style="{ fontFamily: state.selectedFont }">
            <option v-for="font in fonts" :key="font" :value="font">
              {{ font }}
            </option>
          </select>
        </UFormGroup>
        <UButton block type="submit" class="add-btn" icon="i-heroicons-arrow-down-tray">Lae Arve Alla</UButton>
    </div>
  </div>

  </UForm>

</template>

<script setup lang="ts">
  import { ref, watch, defineExpose, onMounted } from 'vue';
  import type { FormErrorEvent } from "#ui/types";
  import { generateInvoicePDF } from '../stores/invoiceStores';
  import { useApi } from '../composables/useApi';
  import type { Invoice } from '../types/invoice'
  import { useProductStore } from '../stores/productStores';
  import { useInvoiceStore } from '../stores/invoiceStores';

  const date = ref(new Date())
  const selectedProducts = ref<Product[]>([]);
  const { customFetch } = useApi();
  const availableProducts = ref<Product[]>([]);
  const companySuggestions = ref<Company[]>([]);
  const pastInvoices = ref<Invoice[]>([]);
  const searchTerm = ref<string>('');
  const { navigateToAddProduct } = useProductStore();
  const { 
          toggleProductSelection, 
          updateQuantity, 
          productDropdownItems, 
          validate, 
          formatDate, 
          formatInvoiceOption,
          onError, 
          state, 
          fonts,} 
        = useInvoiceStore();

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

  const fetchCompanyNames = async () => {
    if (state.title.length < 3) return; 

    try {
      const response = await customFetch<any>(`https://ariregister.rik.ee/est/api/autocomplete?q=${state.title}`, { method: 'GET' })
      companySuggestions.value = response.data;
    } catch (error) {
      console.error('Error fetching company names:', error);
    }
  };

  const filteredProducts = computed(() => {
    if (!searchTerm.value) {
      return availableProducts.value;
    }
    return availableProducts.value.filter(product =>
      product.name.toLowerCase().includes(searchTerm.value.toLowerCase())
    );
  });

  watch(() => state.title, (newTitle) => {
    const selectedCompany = companySuggestions.value.find(company => company.name === newTitle);
    if (selectedCompany) {
      state.clientRegNr = selectedCompany.reg_code;
      state.address = selectedCompany.legal_address;
      state.zipCode = selectedCompany.zip_code;
    }
  });


  watch(() => state.pastInvoice, (selectedInvoiceId) => {
    if (!selectedInvoiceId) return;

    const selectedInvoice = pastInvoices.value.find(invoice => invoice.invoiceId === selectedInvoiceId);

    if (selectedInvoice) {
      state.title = selectedInvoice.title || '';
      state.clientRegNr = selectedInvoice.clientRegNr || '';
      state.clientKMKR = selectedInvoice.clientKMKR || '';
      state.address = selectedInvoice.address || '';
      state.zipCode = String(selectedInvoice.zipCode);
      state.country = selectedInvoice.country || 'Eesti';
      state.invoiceNumber = String(selectedInvoice.invoiceNumber);
      state.dateCreated = formatDate(selectedInvoice.dateCreated || '');
      state.dateDue = formatDate(selectedInvoice.dateDue || '');
      state.condition = selectedInvoice.condition || '';
      state.delayFine = selectedInvoice.delayFine || '';
      state.selectedFont = selectedInvoice.font || '';

      state.productsAndQuantities = selectedInvoice.productsAndQuantities || {};

      const missingProducts = Object.keys(state.productsAndQuantities).filter(productId => {
      return !availableProducts.value.some(product => product.productId === parseInt(productId));
      });

      if (missingProducts.length > 0) {
        window.alert('Hoiatus: Sellel arvel on tooted, mis ei ole enam tootebaasis. Kontrollige soovitud tooted üle.');
      }
      // lisab ka kustutatud tooted kuhugi listi, kopeerides arvet, mis loodi hoiatusega, annab samuti hoiatuse, kuigi tooted on olemas
      selectedProducts.value = Object.keys(state.productsAndQuantities).map(productId => {
        return availableProducts.value.find(product => product.productId === parseInt(productId));
        }).filter(product => product !== undefined) as Product[];
      }
  });

  const submitForm = () => {

    selectedProducts.value.forEach((product) => {
      if (product.productId !== undefined) {
        const quantity = state.productsAndQuantities[product.productId] || 1;
        state.productsAndQuantities[product.productId] = quantity;
      } else {
        console.warn('Invalid product detected:', product);
      }
    });

    generateInvoicePDF(state, "GeneratePdf");
  };


  onMounted(async () => {
  try {
    const response = await customFetch<Product[]>(`Products/all`, { method: 'GET' });
    availableProducts.value = response;
    const invoicesResponse = await customFetch<Invoice[]>(`InvoiceHistory/all`, { method: 'GET' });
    pastInvoices.value = invoicesResponse;
  } catch (error) {
    console.error('Error fetching products:', error);
  }
  });

  defineExpose({ validate, fetchCompanyNames, submitForm });
</script>


<style>
  @import '../assetsFront/styles/main.css';
</style>