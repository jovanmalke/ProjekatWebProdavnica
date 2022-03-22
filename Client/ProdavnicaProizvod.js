export class ProdavnicaProizvod {
    constructor(id, cena, kolicina, alijasProdavnice, alijasProizvoda) {
        this.id = id;
        this.cena = cena;
        this.kolicnaProizvoda = kolicina;
        this.prodavnica = alijasProdavnice;
        this.proizvod = alijasProizvoda;
    }
}