export type PrivatePersonInvoice = {
    invoiceId: number
    name: string
    invoiceNumber: number
    dateCreated: Date
    dateDue: Date
    condition: string
    delayFine: string
    font: string
    products: Product[]
    productsAndQuantities: Record<number, number>
}