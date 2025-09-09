interface Person {
    name: string;
}

interface Employee extends Person {
    employeeId: number;
}

const alice: Employee = {
    name: 'Alice',
    employeeId: 5678
};

console.log(`Employee: ${alice.name}, ID: ${alice.employeeId}`); 
// Output: Employee: Alice, ID: 5678
