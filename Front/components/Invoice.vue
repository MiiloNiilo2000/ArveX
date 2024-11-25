<template>
  <UForm
    :validate="validate"
    :state="state"
    class="space-y-4"
    @submit.prevent="submitForm"
    @error="onError"
  >

  <div class="flex w-full gap-20 h-24 mt-2">
    <div class="w-1/2">
      <UDivider label="Vali enda ettevõte ning arve tüüp" class="h-10 mb-2" />
      <UFormGroup name="invoiceType" class="w-2/2 flex justify-center h-16" >
        <label for="companySelect">Ettevõte:</label>
        <select v-model="selectedCompanyId" @change="onCompanyChange" id="companySelect" class="ml-1">
          <option v-for="company in companyOptions" :key="company.value" :value="company.value">
            {{ company.label }}
          </option>
        </select>
        <label for="invoiceTypeSelect" class="ml-6">Arve tüüp:</label>
        <select v-model="state.invoiceType" @change="onInvoiceTypeChange" id="invoiceTypeSelect" class="ml-1">
          <option value="company">Ettevõte</option>
          <option value="privatePerson">Eraisik</option>
        </select>
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
  <div class="flex w-full gap-20"> 
    <div class="w-1/2"> 
      <UDivider label="Kopeeri varasema arve andmed" class="h-10 mb-2" />
      <UFormGroup name="pastInvoice" class="flex justify-center h-16">
        <select v-model="state.pastInvoice">
          <option value="" disabled>Vali varasem arve:</option>
          <!-- Company invoices -->
          <option v-for="invoice in pastCompanyInvoices" :key="invoice.id" :value="invoice.id">
            {{ formatInvoiceOption(invoice) }}
          </option>
          <!-- Private person invoices -->
          <option v-for="invoice in pastPrivatePersonInvoices" :key="invoice.id" :value="invoice.id">
            {{ formatInvoiceOption(invoice) }}
          </option>
        </select>
      </UFormGroup>
      <UDivider label="Loo uus arve" />
    </div>
  </div>

  <div class="flex gap-20 w-1/2"> 

    <div v-if="state.invoiceType === 'company'" class="w-1/2"> 

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
    
    <div v-if="state.invoiceType === 'privatePerson'" class="w-1/4">
      <UFormGroup label="Kliendi nimi" name="title">
        <UInput
          v-model="state.title"
          class="w-full bg-gray-900 rounded-md mb-4"
          color="emerald"
          placeholder="Sisesta kliendi nimi"
        />
      </UFormGroup>

    </div>

    <div class="w-1/2"> 

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

      
  </div>

  <div class="w-1/2 h-auto">
  <UDivider label="Vali tooted" class="h-10 mb-2" />
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
                  <!-- <div class="ml-6 mt-3">
                    <div>
                      <UDropdown :items="productDropdownItems(product.productId)" :popper="{ offsetDistance: 4, placement: 'right-start' }">
                        <UButton class="mr-2" size="md" color="emerald" variant="ghost" :padded="false" trailing-icon="i-heroicons-ellipsis-horizontal-circle" />
                      </UDropdown>
                    </div>
                  </div> -->
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
  import { useApiForRik } from '../composables/useApiForRik';
  import type { CompanyInvoice } from '../types/companyInvoice'
  import type { PrivatePersonInvoice } from '../types/privatePersonInvoice'
  import { useProductStore } from '../stores/productStores';
  import { useInvoiceStore } from '../stores/invoiceStores';

  const selectedProducts = ref<Product[]>([]);
  const selectedCompanyId = ref<number>();
  const companies = ref<Company[]>([]);
  const { customFetch } = useApi();
  const { customFetchForRik } = useApiForRik();
  const availableProducts = ref<Product[]>([]);
  const companySuggestions = ref<CompanySuggestion[]>([]);
  const pastCompanyInvoices = ref<CompanyInvoice[]>([]);
  const pastPrivatePersonInvoices = ref<PrivatePersonInvoice[]>([]);
  const searchTerm = ref<string>('');
  const { navigateToAddProduct } = useProductStore();
  const {
          validate, 
          formatDate, 
          formatInvoiceOption,
          onError, 
          state, 
          fonts,
          toggleProductSelection, 
          updateQuantity, 
          // productDropdownItems, 
        } 
        = useInvoiceStore();

  interface CompanySuggestion {
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
    quantity: number;
  }  

  const companyOptions = computed(() => {
    return companies.value.map(company => ({
      label: company.name,
      value: company.companyId
    }));
  });

  const fetchProducts = async () => {
    availableProducts.value = [];
    if (selectedCompanyId.value) {
      try {
        const response = await customFetch<Product[]>(`Companies/${selectedCompanyId.value}/Products`, { method: 'GET' });
        availableProducts.value = response;
      } catch (error) {
        console.error("Error fetching companies:", error);
      }
    }
  }

  const fetchCompanies = async () => {
    try {
      const response = await customFetch<Company[]>(`Profile/Companies`, { method: 'GET' });
      companies.value = response;
    } catch (error) {
      console.error("Error fetching companies:", error);
    }
  }; 

  const onCompanyChange = async () => {
    if (selectedCompanyId.value) {
      try {
        await fetchProducts(); // Fetch products specific to the selected company
      } catch (error) {
        console.error("Error fetching products:", error);
      }
    }
  };

  function onInvoiceTypeChange() {
    if (state.invoiceType === 'privatePerson') {
      state.title = '';
      state.clientRegNr = '';
      state.clientKMKR = '';
      state.address = '';
      state.zipCode = '';
      state.country = '';
      state.pastInvoice = null;
      pastCompanyInvoices.value = [];
      loadPastPrivatePersonInvoices();
    } else if (state.invoiceType === 'company') {
      state.title = '';
      state.pastInvoice = null;
      pastPrivatePersonInvoices.value = [];
      loadPastCompanyInvoices();
    }
  }

  const fetchCompanyNames = async () => {
    if (state.title.length < 3) return; 

    try {
      const response = await customFetchForRik<any>(`https://ariregister.rik.ee/est/api/autocomplete?q=${state.title}`, { method: 'GET' })
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

  const loadPastCompanyInvoices = async () => {
    try {
      const response = await customFetch<CompanyInvoice[]>(`InvoiceHistory/all`, { method: 'GET' });
      pastCompanyInvoices.value = response.filter(invoice => invoice.invoiceType === 'company');
      console.log('Company Invoices:', pastCompanyInvoices.value);
    } catch (error) {
      console.error('Error fetching company invoices:', error);
    }
  };

  const loadPastPrivatePersonInvoices = async () => {
    try {
      const response = await customFetch<PrivatePersonInvoice[]>(`InvoiceHistory/all`, { method: 'GET' });
      pastPrivatePersonInvoices.value = response.filter(invoice => invoice.invoiceType === 'privatePerson');
    } catch (error) {
      console.error('Error fetching private person invoices:', error);
    }
  };
  
  watch(() => state.pastInvoice, async (selectedInvoiceId) => {
    if (!selectedInvoiceId) return;

    let selectedInvoice;

    if (state.invoiceType === 'company') {
      selectedInvoice = pastCompanyInvoices.value.find(invoice => invoice.id === selectedInvoiceId);
    } else if (state.invoiceType === 'privatePerson') {
      selectedInvoice = pastPrivatePersonInvoices.value.find(invoice => invoice.id === selectedInvoiceId);
    }
    

    if (selectedInvoice) {
      state.title = selectedInvoice.title || '';

      state.invoiceNumber = String(selectedInvoice.invoiceNumber);
      state.dateCreated = formatDate(selectedInvoice.dateCreated || '');
      state.dateDue = formatDate(selectedInvoice.dateDue || '');
      state.condition = selectedInvoice.condition || '';
      state.delayFine = selectedInvoice.delayFine || '';
      state.selectedFont = selectedInvoice.font || '';

      if (state.invoiceType === 'company' && 'clientRegNr' in selectedInvoice) {
        state.clientRegNr = selectedInvoice.clientRegNr || '';
        state.clientKMKR = selectedInvoice.clientKMKR || '';
        state.address = selectedInvoice.address || '';
        state.zipCode = String(selectedInvoice.zipCode);
        state.country = selectedInvoice.country || 'Eesti';
      }
      
      // lisab ka kustutatud tooted kuhugi listi, kopeerides arvet, mis loodi hoiatusega, annab samuti hoiatuse, kuigi tooted on olemas
      const invoiceProducts = await fetchProductsForInvoice(selectedInvoiceId);
      selectedProducts.value = invoiceProducts;

      state.productsAndQuantities = selectedProducts.value.reduce<{ [key: number]: number }>((acc, product) => {
        if (product.productId && product.quantity) {
          acc[product.productId] = product.quantity;
        }
        return acc;
      }, {});
      
      const missingProducts = Object.keys(state.productsAndQuantities).filter(productId => {
      return !availableProducts.value.some(product => product.productId === parseInt(productId));
      });

      if (missingProducts.length > 0) {
        window.alert('Hoiatus: Sellel arvel on tooted, mis ei ole enam tootebaasis. Kontrollige soovitud tooted üle.');
      }

      }
  });

  const fetchProductsForInvoice = async (invoiceId: number) => {
    try {
      const response = await customFetch<Product[]>(`InvoiceHistory/${invoiceId}/products`, { method: 'GET' });
      return response;
    } catch (error) {
      console.error("Error fetching products for past invoice:", error);
      return [];
    }
  };

  const submitForm = () => {

    selectedProducts.value.forEach((product) => {
      if (product.productId !== undefined) {
        const quantity = state.productsAndQuantities[product.productId] || 1;
        state.productsAndQuantities[product.productId] = quantity;
      } else {
        console.warn('Invalid product detected:', product);
      }
    });
    const route = state.invoiceType === "privatePerson" ? "privatePerson" : "company";
    generateInvoicePDF(state, route);

  };

  onMounted(async () => {
  try {
    state.invoiceType = 'company';
    await loadPastCompanyInvoices();
    await customFetch<Product[]>(`Products/all`, { method: 'GET' });
    await fetchCompanies();
    if (companies.value.length > 0) {
      selectedCompanyId.value = companies.value[0].companyId;
      fetchProducts();
    }
  } catch (error) {
    console.error('Error fetching products:', error);
  }
  });

  defineExpose({ validate, fetchCompanyNames, submitForm });
</script>

<style>
  @import '../assetsFront/styles/main.css';
</style>