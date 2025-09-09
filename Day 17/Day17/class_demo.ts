class Trainer {
    name: string;
    experience: number;
  
    constructor(name: string, experience: number) {
      this.name = name;
      this.experience = experience;
    }
  
    introduce(): void {
      console.log(`Hi, I'm ${this.name} with ${this.experience} years of experience.`);
    }
  
    calculateSessions(): number {
      return this.experience * 75;
    }
  }
  
  const sushma = new Trainer("Jyoti", 16);
  sushma.introduce(); // Output: Hi, I'm Jyoti with 16 years of experience.
  console.log(sushma.calculateSessions()); // Output: 1200
  