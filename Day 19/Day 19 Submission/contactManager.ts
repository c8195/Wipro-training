// src/contactManager.ts
console.log("🚀 contactManager.ts is running...");

// 1) Interface for Contact
interface Contact {
    id?: number;         // optional so we can auto-assign if not provided
    name: string;
    email: string;
    phone: string;
  }
  
  // 2) ContactManager class
  class ContactManager {
    private contacts: Contact[] = [];
    private nextId: number = 1;
  
    // addContact(contact: Contact): void
    addContact(contact: Contact): void {
      // auto-assign id if not provided
      if (contact.id == null) {
        contact.id = this.nextId++;
      } else {
        // if id provided and already exists => error
        if (this.contacts.some(c => c.id === contact.id)) {
          console.error(`Error: Contact with id ${contact.id} already exists.`);
          return;
        }
        // keep nextId ahead of any provided id
        if (contact.id >= this.nextId) this.nextId = contact.id + 1;
      }
  
      this.contacts.push(contact as Contact);
      console.log(`✅ Success: Contact added (id: ${contact.id}).`);
    }
  
    // viewContacts(): Contact[]
    viewContacts(): Contact[] {
      return this.contacts;
    }
  
    // modifyContact(id: number, updatedContact: Partial<Contact>): void
    modifyContact(id: number, updatedContact: Partial<Contact>): void {
      const contact = this.contacts.find(c => c.id === id);
      if (!contact) {
        console.error(`❌ Error: Contact with id ${id} not found.`);
        return;
      }
      // merge updates (id cannot be changed)
      Object.assign(contact, updatedContact);
      contact.id = id;
      console.log(`✅ Success: Contact ${id} updated.`);
    }
  
    // deleteContact(id: number): void
    deleteContact(id: number): void {
      const idx = this.contacts.findIndex(c => c.id === id);
      if (idx === -1) {
        console.error(`❌ Error: Contact with id ${id} not found.`);
        return;
      }
      this.contacts.splice(idx, 1);
      console.log(`✅ Success: Contact ${id} deleted.`);
    }
  }
  
  /* 4) Simple test script */
  
  // create manager
  const manager = new ContactManager();
  
  // add contacts
  manager.addContact({ name: "Alice", email: "alice@example.com", phone: "111-222-3333" });
  manager.addContact({ name: "Bob", email: "bob@example.com", phone: "222-333-4444" });
  
  // view all
  console.log("\n📋 All contacts after adding:");
  console.table(manager.viewContacts());
  
  // modify a contact
  manager.modifyContact(1, { phone: "999-999-9999" });
  console.log("\n📋 Contacts after modifying id=1:");
  console.table(manager.viewContacts());
  
  // delete a contact
  manager.deleteContact(2);
  console.log("\n📋 Contacts after deleting id=2:");
  console.table(manager.viewContacts());
  
  // attempt to modify/delete a non-existent contact (shows error handling)
  manager.modifyContact(99, { name: "NonExistent" });
  manager.deleteContact(99);
  