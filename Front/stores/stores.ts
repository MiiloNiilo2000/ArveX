import type { Invoice } from "~/types/invoice";

export const useInvoiceStore = defineStore('invoice', () => {
    const invoices = ref<Invoice[]>([]);
      
    const generateId = () => {
      const randomDigits = Math.floor(100000 + Math.random() * 900000).toString(); 
      return randomDigits
    }; 
      return { invoices };
  })