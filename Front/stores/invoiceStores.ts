import type { FormError, FormErrorEvent } from "#ui/types";
import { useApi } from '../composables/useApi';

export async function generateInvoicePDF(state: any, routeName: string) {
  const { customFetch } = useApi();
  try {
    let payload;
    if(routeName === "company"){
      payload = {
        title: state.title,
        clientRegNr: state.clientRegNr.toString(),
        clientKMKR: state.clientKMKR,
        address: state.address,
        zipCode: state.zipCode.toString(),
        country: state.country,
        invoiceNumber: parseInt(state.invoiceNumber),
        dateCreated: new Date(state.dateCreated).toISOString(),
        dateDue: new Date(state.dateDue).toISOString(),
        condition: state.condition || "",
        delayFine: state.delayFine || "",
        font: state.selectedFont,
        invoiceType: state.invoiceType,
        // productsAndQuantitiesJson: JSON.stringify(state.productsAndQuantities),
      };
    }
    else {
      payload = {
        title: state.title,
        invoiceNumber: parseInt(state.invoiceNumber),
        dateCreated: new Date(state.dateCreated).toISOString(),
        dateDue: new Date(state.dateDue).toISOString(),
        condition: state.condition || "",
        delayFine: state.delayFine || "",
        font: state.selectedFont,
        invoiceType: state.invoiceType,
        // productsAndQuantitiesJson: JSON.stringify(state.productsAndQuantities),
      };
    }

    if(!payload) {
      throw new Error("Invalid routeName or missing data for payload.");
    }
    
    const response = await customFetch<Blob>(`/CreateInvoice/GeneratePdf${routeName}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });

    const url = window.URL.createObjectURL(new Blob([response]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', 'invoice.pdf');
    document.body.appendChild(link);
    link.click(); 
    link.remove();
  } catch (error) {
      console.error("Error message:", error);
  }
}


export const useInvoiceStore = defineStore('invoice', () => {

  const selectedProducts = ref<Product[]>([]);
  const availableProducts = ref<Product[]>([]);
  const companySuggestions = ref<Company[]>([]);
  const pastInvoices = ref<CompanyInvoice[]>([]);
  const { navigateToEditProduct } = useProductStore();

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
    clientRegNr: '',
    clientKMKR: '',
    address: '',
    zipCode: '',
    country: 'Eesti',
    invoiceNumber: '',
    dateCreated: new Date().toISOString().split('T')[0],
    dateDue: '',
    condition: '',
    delayFine: '',
    selectedFont: 'Arial',
    footerImage: null,
    productsAndQuantities: {} as Record<number, number>,
    pastInvoice: null,
    invoiceType: '',
  });

  const toggleProductSelection = (productId: number) => {
    if (state.productsAndQuantities[productId] !== undefined) {
      delete state.productsAndQuantities[productId];
      selectedProducts.value = selectedProducts.value.filter(product => product.productId !== productId);
    } else {
      state.productsAndQuantities[productId] = 1;
      const product = availableProducts.value.find(p => p.productId === productId);
      if (product) {
        selectedProducts.value.push(product);
      }
    }
  };

  const updateQuantity = (productId: number) => {
    if (state.productsAndQuantities[productId] < 1) {
      state.productsAndQuantities[productId] = 1;
    }
  };

  const productDropdownItems = (productId: number) => [
    [ 
      {
        label: 'Muuda seda toodet',
        click: () => {
          navigateToEditProduct(productId);
        },
      },
    ]
  ];

  function formatDate(dateInput: string | Date): string {
    const date = typeof dateInput === 'string' ? new Date(dateInput) : dateInput;
    if (isNaN(date.getTime())) return ''; 
    return date.toISOString().split('T')[0]; 
  }

  function formatInvoiceOption(invoice: CompanyInvoice | PrivatePersonInvoice): string {
    const dateCreated = new Date(invoice.dateCreated).toISOString().split('T')[0]; 
    return `${invoice.title} - Nr: ${invoice.invoiceNumber} (${dateCreated})`;
  }

  const validate = (state: any): FormError[] => {
    const errors = [];
    if (state.invoiceType == 'company'){
      const zipString = state.zipCode.toString();
      if (!state.clientKMKR) errors.push({ path: "clientKMKR", message: "Required" });
      if (!state.clientRegNr) errors.push({ path: "clientRegNr", message: "Required" });
      if (!state.address) errors.push({ path: "address", message: "Required" });
      if (!state.zipCode) errors.push({ path: "zipCode", message: "Required" });
      if (zipString.length < 5 || zipString.length > 5) errors.push({ path: "zipCode", message: "Postiindeks peab olema 5-kohaline number" });
    }
    if (!state.title) errors.push({ path: "title", message: "Required" });
    if (!state.invoiceNumber) errors.push({ path: "invoiceNr", message: "Required" });
    if (!state.dateCreated) errors.push({ path: "dateCreated", message: "Required" });
    if (!state.dateDue) errors.push({ path: "dateDue", message: "Required" });  
  
    return errors;
  };

  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }

  const fonts = [
    'Times New Roman',
    'Arial',
    'Courier New',
    'Georgia',
    'Verdana',
  ]
  
  return {  toggleProductSelection, 
            updateQuantity, 
            productDropdownItems, 
            validate, formatDate, 
            formatInvoiceOption, 
            onError, 
            state, 
            selectedProducts, 
            availableProducts, 
            companySuggestions, 
            pastInvoices, 
            fonts
          };
});