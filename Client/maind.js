import { Prodavnica } from "./Prodavnica.js";
import { Proizvod } from "./Proizvod.js";
import { ProdavnicaProizvod } from "./ProdavnicaProizvod.js"
import { Dostavljac } from "./Dostavljac.js"

let listaProdavnica = [];
let listaProizvoda = [];
let listaProizvodaUProdavnici = [];
let listaDostavljaca = [];

ucitajProdavnice();
ucitajProizvode();

napraviGlavnuFormu();

function ucitajDostavljace(idP) {
    fetch("https://localhost:5001/Dostavljac/PreuzmiDostavljace/" + idP)
        .then(x => {
            x.json()
                .then(dostavljaci => {
                    listaDostavljaca.length = 0;
                    dostavljaci.forEach(dostavljac => {
                        let pom = new Dostavljac(dostavljac.id, dostavljac.ime, dostavljac.prezime, dostavljac.godine, dostavljac.brTelefona, dostavljac.cenaUsluge, dostavljac.alijasProdavnice);
                        listaDostavljaca.push(pom);
                    });
                    popuniTabelu();
                })
        })
}

function upisiProdavnice() {
    let slProdavnice = document.querySelector(".imeProdavnice");
    let slProdavniceFormular = document.querySelector(".imeProdavniceFormular");
    let p1;
    let p2;
    listaProdavnica.forEach(prodavnica => {
        p1 = document.createElement("option");
        p1.innerHTML = prodavnica.naziv;
        p1.value = prodavnica.id;
        slProdavnice.appendChild(p1);
        p2 = document.createElement("option");
        p2.innerHTML = prodavnica.naziv;
        p2.value = prodavnica.id;
        slProdavniceFormular.appendChild(p2);
    });
}

function ucitajProizvodeUProdavnici() {
    let prod = document.querySelector(".imeProdavnice").value;
    fetch("https://localhost:5001/ProdavnicaProizvod/PreuzmiImena/" + prod, { method: "GET" })
        .then(x => {
            x.json()
                .then(proizvodi => {
                    listaProizvodaUProdavnici.length = 0;
                    proizvodi.forEach(proizvod => {
                        let pom = new ProdavnicaProizvod(proizvod.id, proizvod.cena, proizvod.kolicina, proizvod.alijasProdavnice, proizvod.alijasProizvoda);
                        listaProizvodaUProdavnici.push(pom);
                    });
                    console.log(listaProizvodaUProdavnici);
                })
        })
}

function ucitajProdavnice() {
    fetch("https://localhost:5001/Prodavnica/PreuzmiIme")
        .then(x => {
            x.json()
                .then(prodavnice => {
                    prodavnice.forEach(prodavnica => {
                        let pom = new Prodavnica(prodavnica.id, prodavnica.naziv, prodavnica.adresa, prodavnica.prodavnicaNabavljac);
                        listaProdavnica.push(pom);
                    });
                    upisiProdavnice();
                })
        })
}

function ucitajProizvode() {
    fetch("https://localhost:5001/Proizvod/PreuzmiImena")
        .then(x => {
            x.json()
                .then(proizvodi => {
                    proizvodi.forEach(proizvod => {
                        let pom = new Proizvod(proizvod.id, proizvod.ime);
                        listaProizvoda.push(pom);
                    });
                    let goreDesnoForma = document.querySelector(".goreDesnoForma");
                    napraviGoreDesnoFormu(goreDesnoForma);
                })
        })
}

function napraviGlavnuFormu() {
    let glavniDiv = document.createElement("div");
    glavniDiv.className = "glavniDiv";
    document.body.appendChild(glavniDiv);

    let goreForma = document.createElement("div");
    goreForma.className = "goreForma";
    glavniDiv.appendChild(goreForma);

    let doleForma = document.createElement("div");
    doleForma.className = "doleForma";
    glavniDiv.appendChild(doleForma);

    napraviGornjuFormu(goreForma);
    napraviDonjuFormu(doleForma);
}

function napraviGornjuFormu(host) {
    let goreLevoForma = document.createElement("div");
    goreLevoForma.className = "goreLevoForma";
    host.appendChild(goreLevoForma);

    let goreDesnoForma = document.createElement("div");
    goreDesnoForma.className = "goreDesnoForma";
    host.appendChild(goreDesnoForma);

    let selProdavnice = document.createElement("select");
    selProdavnice.className = "imeProdavnice";
    goreLevoForma.appendChild(selProdavnice);

    let btn = document.createElement("button");
    btn.className = "btnDodajProizvod";
    let txt = document.createTextNode("Dodaj proizvode");
    btn.appendChild(txt);
    goreLevoForma.appendChild(btn);

    let formaZaNabavljaca = document.createElement("div");
    formaZaNabavljaca.className = "formaZaNabavljaca";
    goreLevoForma.appendChild(formaZaNabavljaca);

    napraviGoreDesnoFormu(goreDesnoForma);

    btn.onclick = (ev) => prikazZaNabavljaca(formaZaNabavljaca);
}

function napraviDonjuFormu(host) {
    let doleLevoForma = document.createElement("div");
    doleLevoForma.className = "doleLevoForma";
    host.appendChild(doleLevoForma);

    let doleDesnoForma = document.createElement("div");
    doleDesnoForma.className = "doleDesnoForma";
    host.appendChild(doleDesnoForma);

    napraviFormular(doleLevoForma);
    napraviTabelu(doleDesnoForma);
}

function napraviTabelu(host) {
    let tabela = document.createElement("table");
    tabela.className = "tabela";
    host.appendChild(tabela);

    let thead = document.createElement("thead");
    tabela.appendChild(thead);

    let tr = document.createElement("tr");
    tr.className = "row";
    thead.appendChild(tr);

    let tbody = document.createElement("tbody");
    tbody.className = "tbody";
    tabela.appendChild(tbody);

    let th;
    let nizZaglavlja = ["Ime", "Prezime", "Godine", "Broj telefona", "Cena usluge"];
    nizZaglavlja.forEach(el => {
        th = document.createElement("th");
        th.innerHTML = el;
        tr.appendChild(th);
    })
    let btn = document.createElement("button");
    btn.className = "dugmeIzbrisi";
    let txt = document.createTextNode("Izbrisi");
    btn.appendChild(txt);
    btn.onclick = (ev) => izbrisiDostavljaca();
    host.appendChild(btn);
    btn.disabled = true;
    btn = document.createElement("button");
    btn.className = "dugmeIzmeni";
    txt = document.createTextNode("Izmeni");
    btn.appendChild(txt);
    host.appendChild(btn);
    btn.disabled = true;
}

function napraviFormular(host) {
    let selProdavnice = document.createElement("select");
    selProdavnice.className = "imeProdavniceFormular";
    host.appendChild(selProdavnice);
    let btnPrikazi = document.createElement("button");
    btnPrikazi.className = "btnPrikazi";
    let txt = document.createTextNode("Prikazi");
    btnPrikazi.appendChild(txt);
    host.appendChild(btnPrikazi);
    btnPrikazi.onclick = (ev) => prikaziTabelu(selProdavnice.value);
    let pom;
    let pomLab;
    let nizLabela = ["Ime", "Prezime", "BrojTelefona"];
    nizLabela.forEach(el => {
        pomLab = document.createElement("label");
        pomLab.innerHTML = el;
        pom = document.createElement("input");
        pom.type = "text";
        pom.className = el + "Tekst";
        if (el === "BrojTelefona") {
            pom.placeholder = "+381";
        }
        else {
            pom.placeholder = el;
        }
        host.appendChild(pomLab);
        host.appendChild(pom);
    })
    nizLabela = ["Godine", "Cena"];
    nizLabela.forEach(el => {
        pomLab = document.createElement("label");
        pomLab.innerHTML = el;
        pom = document.createElement("input");
        pom.type = "number";
        pom.className = el + "Broj";
        if (el === "Godine") {
            pom.min = 18;
            pom.max = 60;
        }
        else {
            pom.min = 100;
            pom.max = 1000;
        }
        host.appendChild(pomLab);
        host.appendChild(pom);
    })
    let btn = document.createElement("button");
    btn.className = "dodajDostavljaca";
    btn.onclick = (ev) => dodajDostavljacaProdavnici();
    txt = document.createTextNode("Dodaj dostavljaca");
    btn.appendChild(txt);
    host.appendChild(btn);
}

function dodajDostavljacaProdavnici() {
    let imeD = document.querySelector(".ImeTekst").value;
    let prezimeD = document.querySelector(".PrezimeTekst").value;
    let brtelD = document.querySelector(".BrojTelefonaTekst").value;
    let godineD = document.querySelector(".GodineBroj").value;
    let cenaD = document.querySelector(".CenaBroj").value;
    let idP = document.querySelector(".imeProdavniceFormular").value;
    let phoneno = "^\\s*\\+?\\s*([0-9][\\s-]*){9,}$";
    let neValidan = false;
    if (brtelD.match(phoneno)) {
        neValidan = false;
    }
    else {
        neValidan = true;
    }
    console.log(neValidan);
    if (neValidan || (imeD === "" || prezimeD === "")) {
        alert("Nevalidne vrednosti");
    }
    else {
        fetch("https://localhost:5001/Dostavljac/DodajDostavljaca/" + imeD + "/" + prezimeD + "/" + brtelD + "/" + godineD + "/" + cenaD + "/" + idP, { method: "POST" })
            .then(x => {
                if (x.ok) {
                    alert("Uspesno dodat");
                    prikaziTabelu(idP);
                }
                else {
                    alert("Nevalidni podaci");
                }
            })
    }
}

function prikaziTabelu(prodavnica) {
    ucitajDostavljace(prodavnica);
    console.log(listaDostavljaca);
}

function prikazZaNabavljaca(host) {
    let goreDesnoForma = document.querySelector(".goreDesnoForma");
    napraviGoreDesnoFormu(goreDesnoForma);
    obrisiFormu(host);
    ucitajProizvodeUProdavnici();
    console.log(listaProizvodaUProdavnici);
    let pom;
    let pomLab;
    pomLab = document.createElement("label");
    pomLab.innerHTML = "Ime nabavljaca";
    pom = document.createElement("input");
    pom.type = "text";
    pom.className = "imeNabavljaca";
    let imeProd = document.querySelector(".imeProdavnice").value;
    let prod = listaProdavnica.find(x => x.id == imeProd);
    pom.value = prod.prodavnicaNabavljac.ime + " " + prod.prodavnicaNabavljac.prezime;
    pom.disabled = true;
    host.appendChild(pomLab);
    host.appendChild(pom);
    pomLab = document.createElement("label");
    pomLab.innerHTML = "Sifra nabavljaca";
    host.appendChild(pomLab);
    pom = document.createElement("input");
    pom.type = "password";
    pom.className = "sifraNabavljaca";
    pom.placeholder = "Sifra";
    host.appendChild(pom);
    let btnZaProveru = document.createElement("button");
    btnZaProveru.className = "btnZaProveru";
    host.appendChild(btnZaProveru);
    let txt = document.createTextNode("Provera");
    btnZaProveru.appendChild(txt);
    btnZaProveru.onclick = (ev) => {
        if (document.querySelector("input[type='password']").value === prod.prodavnicaNabavljac.sifra) {
            console.log(prod.prodavnicaNabavljac);
            let pom = document.querySelectorAll(".cenaKolicina");
            for (let i = 0; i < pom.length; i++) {
                pom[i].disabled = false;
            }
            pom = document.querySelectorAll(".dugmeDodaj");
            for (let i = 0; i < pom.length; i++) {
                pom[i].disabled = false;
            }
            listaProizvodaUProdavnici.forEach(x => {
                pom = document.querySelector("#" + x.proizvod + "Kolicina");
                //pom = document.getElementById(x.proizvod + "Kolicina");
                pom.value = x.kolicnaProizvoda;
                pom.min = x.kolicnaProizvoda;
                pom.max = 10000;
                pom = document.querySelector("#" + x.proizvod + "Cena");
                //pom = document.getElementById(x.proizvod + "Cena");
                pom.value = x.cena;
            })
        }
        else {
            let sifra = document.querySelector(".sifraNabavljaca");
            sifra.value = "";
            alert("Pogresna sifra");
        }
    }
}

function popuniTabelu() {
    let teloTabele = document.querySelector(".tbody");
    let roditeljTabela = teloTabele.parentNode;
    roditeljTabela.removeChild(teloTabele);

    teloTabele = document.createElement("tbody");
    teloTabele.className = "tbody";
    roditeljTabela.appendChild(teloTabele);

    listaDostavljaca.forEach(el => {

        let tr = document.createElement("tr");
        tr.className = "red";
        teloTabele.appendChild(tr);

        tr.id = el.id;

        tr.addEventListener("dblclick", () => {
            tr.className = "neselektovan";
            let btn = document.querySelector(".dugmeIzmeni");
            btn.disabled = true;
            btn = document.querySelector(".dugmeIzbrisi");
            btn.disabled = true;
            let pomTekst = document.querySelector(".ImeTekst");
            pomTekst.value = "";
            pomTekst = document.querySelector(".PrezimeTekst")
            pomTekst.value = "";
            pomTekst = document.querySelector(".BrojTelefonaTekst")
            pomTekst.value = "";
            pomTekst = document.querySelector(".CenaBroj");
            pomTekst.value = pomTekst.min;
            pomTekst = document.querySelector(".GodineBroj");
            pomTekst.value = pomTekst.min;
        });
        tr.addEventListener("click", () => {
            tr.className = "selektovan";
            let btn = document.querySelector(".dugmeIzmeni");
            btn.disabled = false;
            btn.onclick = (ev) => izmeniDostavljaca(tr.id);
            btn = document.querySelector(".dugmeIzbrisi");
            btn.disabled = false;
            btn.onclick = (ev) => izbrisiDostavljaca(tr.id);
            let pomTekst = document.querySelector(".ImeTekst");
            pomTekst.value = el.ime;
            pomTekst = document.querySelector(".PrezimeTekst")
            pomTekst.value = el.prezime;
            pomTekst = document.querySelector(".BrojTelefonaTekst")
            pomTekst.value = el.brTelefona;
            pomTekst = document.querySelector(".CenaBroj");
            pomTekst.value = el.cenaUsluge;
            pomTekst = document.querySelector(".GodineBroj");
            pomTekst.value = el.godine;
        });

        let ime = document.createElement("td");
        ime.innerHTML = el.ime;
        tr.appendChild(ime);

        let prezime = document.createElement("td");
        prezime.innerHTML = el.prezime;
        tr.appendChild(prezime);

        let godine = document.createElement("td");
        godine.innerHTML = el.godine;
        tr.appendChild(godine);

        let brtelefona = document.createElement("td");
        brtelefona.innerHTML = el.brTelefona;
        tr.appendChild(brtelefona);

        let cena = document.createElement("td");
        cena.innerHTML = el.cenaUsluge;
        tr.appendChild(cena);
    });
}

function izmeniDostavljaca(idD) {
    let imeD = document.querySelector(".ImeTekst").value;
    let prezimeD = document.querySelector(".PrezimeTekst").value;
    let brtelD = document.querySelector(".BrojTelefonaTekst").value;
    let godineD = document.querySelector(".GodineBroj").value;
    let cenaD = document.querySelector(".CenaBroj").value;
    let idP = document.querySelector(".imeProdavniceFormular").value;
    let phoneno = "^\\s*\\+?\\s*([0-9][\\s-]*){9,}$";
    let neValidan = false;
    if (brtelD.match(phoneno)) {
        neValidan = false;
    }
    else {
        neValidan = true;
    }
    console.log(neValidan);
    if (neValidan || (imeD === "" || prezimeD === "")) {
        alert("Nevalidne vrednosti");
    }
    else {
        fetch("https://localhost:5001/Dostavljac/IzmeniDostavljaca/" + idD + "/" + imeD + "/" + prezimeD + "/" + godineD + "/" + brtelD + "/" + cenaD, { method: "PUT" })
            .then(x => {
                if (x.ok) {
                    alert("Uspesno izmenjen");
                    prikaziTabelu(idP);
                }
                else {
                    alert("Nije uspela izmena");
                }

            })
    }
}

function izbrisiDostavljaca(idD) {
    let idP = document.querySelector(".imeProdavniceFormular").value;
    fetch("https://localhost:5001/Dostavljac/IzbrisiDostavljaca/" + idD, { method: "DELETE" })
        .then(x => {
            if (x.ok) {
                alert("Uspesno obrisan");
                prikaziTabelu(idP);
            }
            else {
                alert("Nije uspelo brisanje");
            }

        })
}

function napraviGoreDesnoFormu(host) {
    obrisiFormu(host);
    let pom;
    let pomDivKol;
    let pomDivCena;
    let slika;
    let labCena;
    let labKol;
    let txtKol;
    let txtCena;
    let btn;
    let txt;
    listaProizvoda.forEach(x => {
        pom = document.createElement("div");
        pom.className = "divProizvoda";
        pomDivKol = document.createElement("div");
        pomDivKol.className = "divKolicina";
        pomDivCena = document.createElement("div");
        pomDivCena.className = "divCena";
        host.appendChild(pom);
        slika = document.createElement("img");
        slika.className = "slike"
        slika.src = "../Slike/" + x.ime + ".jpg";
        slika.alt = x.ime + ".jpg";
        labCena = document.createElement("label");
        labKol = document.createElement("label");
        txtCena = document.createElement("input");
        txtCena.className = "cenaKolicina";
        txtCena.id = x.ime + "Cena";
        txtCena.disabled = true;
        txtKol = document.createElement("input");
        txtKol.className = "cenaKolicina";
        txtKol.id = x.ime + "Kolicina";
        txtKol.disabled = true;
        txtCena.type = "number";
        txtCena.min = 1;
        txtKol.type = "number";
        txtKol.min = 1;
        labCena.innerHTML = "Cena: ";
        labCena.value = "Cena: ";
        labKol.innerHTML = "Kolicina: ";
        labKol.value = "Kolicina: ";
        pom.appendChild(pomDivKol);
        pomDivKol.appendChild(labKol);
        pomDivKol.appendChild(txtKol);
        pom.appendChild(slika);
        pom.appendChild(pomDivCena);
        pomDivCena.appendChild(labCena);
        pomDivCena.appendChild(txtCena);
        btn = document.createElement("button");
        btn.className = "dugmeDodaj";
        btn.id = x.ime + "DugmeDodaj";
        console.log(x.ime);
        btn.onclick = (ev) => dodajProizvodUProdavnicu(x.ime);
        btn.disabled = true;
        txt = document.createTextNode("Dodaj proizvod");
        btn.appendChild(txt);
        pom.appendChild(btn);
    })
}

function obrisiFormu(forma) {
    while (forma.firstChild) {
        forma.removeChild(forma.firstChild);
    }
}

function dodajProizvodUProdavnicu(proizvod) {
    console.log(proizvod);
    let kol = document.querySelector("#" + proizvod + "Kolicina").value;
    console.log(kol);
    let cena = document.querySelector("#" + proizvod + "Cena").value;
    let imeProd = document.querySelector(".imeProdavnice").value;
    let nadjen = false;
    listaProizvodaUProdavnici.forEach(x => {
        if (x.proizvod == proizvod) {
            nadjen = true;
        }
    })
    if (!nadjen) {
        console.log("NIJE NADJEN");
        fetch("https://localhost:5001/ProdavnicaProizvod/DodajProizvodUProdavnicu/" + imeProd + "/" + proizvod + "/" + kol + "/" + cena, { method: "POST" })
            .then(x => {
                if (x.ok) {
                    console.log("Uspesno dodat");
                }
            })
    }
    else {
        console.log("NADJEN");
        fetch("https://localhost:5001/ProdavnicaProizvod/IzmeniProizvodUProdavnici/" + imeProd + "/" + proizvod + "/" + kol + "/" + cena, { method: "PUT" })
            .then(x => {
                if (x.ok) {
                    console.log("Uspesno izmenjen");
                }

            })
    }
}