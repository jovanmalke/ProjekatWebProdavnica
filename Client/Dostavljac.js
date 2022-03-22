export class Dostavljac {
    constructor(id, ime, prezime, godine, brtel, cena, alijasProdavnice) {
        this.id = id;
        this.ime = ime;
        this.prezime = prezime;
        this.godine = godine;
        this.brTelefona = brtel;
        this.dostavljacProdavnica = alijasProdavnice;
        this.cenaUsluge = cena;
    }
}