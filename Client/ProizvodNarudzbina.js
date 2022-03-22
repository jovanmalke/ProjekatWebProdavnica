export class ProizvodNarudzbina {
    constructor(kolicina, proizvod, c, id, narudzbina) {
        this.kolicina = kolicina;
        this.proizvod = proizvod;
        this.id = id;
        this.narudzbina = narudzbina;
        this.cena = c;
    }
    azurirajPostojeciProizvod(kolicina, c) {
        this.kolicina = kolicina;
        this.cena = c;
    }
}