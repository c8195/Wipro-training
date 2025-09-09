var Trainer = /** @class */ (function () {
    function Trainer(name, experience) {
        this.name = name;
        this.experience = experience;
    }
    Trainer.prototype.introduce = function () {
        console.log("Hi, I'm ".concat(this.name, " with ").concat(this.experience, " years of experience."));
    };
    Trainer.prototype.calculateSessions = function () {
        return this.experience * 75;
    };
    return Trainer;
}());
var sushma = new Trainer("Jyoti", 16);
sushma.introduce(); // Output: Hi, I'm Jyoti with 16 years of experience.
console.log(sushma.calculateSessions()); // Output: 1200
