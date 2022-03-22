import { Prodavnica } from "./Prodavnica.js";
import { ProdavnicaProizvod } from "./ProdavnicaProizvod.js"
import { Dostavljac } from "./Dostavljac.js"
import { ProizvodNarudzbina } from "./ProizvodNarudzbina.js"
import { Narudzbina } from "./Narudzbina.js";

let listaProdavnica = [];
let listaProizvodaUProdavnici = [];
let listaDostavljaca = [];
let listaProizvodaUNarudzbini = [];
let listaNarudzbina = [];
let glavnaNarudzbina;

ucitajProdavnice();

napraviGlavnuFormu();

function upisiProdavnice() {
    let slProdavnice = document.querySelector(".imeProdavnice");
    let p;
    listaProdavnica.forEach(prodavnica => {
        p = document.createElement("option");
        p.innerHTML = prodavnica.naziv;
        p.value = prodavnica.naziv;
        slProdavnice.appendChild(p);
    });
}

function ucitajNarudzbine() {
    fetch("https://localhost:5001/Narudzbina/PreuzmiNarudzbu")
        .then(x => {
            x.json()
                .then(narudzbine => {
                    narudzbine.forEach(narudzbina => {
                        let pom = new Narudzbina(narudzbina.id, narudzbina.idProdavnice);
                        listaNarudzbina.push(pom);
                    });
                    crtajGrafik();
                })
        })
}

function vratiNarudzbinu() {
    let adresa = document.querySelector(".adresa").value;
    let brTelefona = document.querySelector(".brTelefona").value;
    fetch("https://localhost:5001/Narudzbina/PreuzmiNarudzbu/" + adresa + "/" + brTelefona)
        .then(x => {
            x.json()
                .then(narudzbina => {
                    glavnaNarudzbina = new Narudzbina(narudzbina.id, narudzbina.idp);
                })
        })
}

function ucitajProdavnice() {
    fetch("https://localhost:5001/Prodavnica/PreuzmiIme")
        .then(x => {
            x.json()
                .then(prodavnice => {
                    prodavnice.forEach(prodavnica => {
                        let pom = new Prodavnica(prodavnica.id, prodavnica.naziv, prodavnica.adresa);
                        listaProdavnica.push(pom);
                    });
                    ucitajNarudzbine();
                    upisiProdavnice();
                })
        })
}

function ucitajProizvodeUProdavnici() {
    let imeProd = document.querySelector(".imeProdavnice").value;
    fetch("https://localhost:5001/ProdavnicaProizvod/PreuzmiImenaVeceOdNula/" + imeProd)
        .then(x => {
            x.json()
                .then(proizvodi => {
                    listaProizvodaUProdavnici.length = 0;
                    proizvodi.forEach(proizvod => {
                        let pom = new ProdavnicaProizvod(proizvod.id, proizvod.cena, proizvod.kolicina, proizvod.alijasProdavnice, proizvod.alijasProizvoda);
                        listaProizvodaUProdavnici.push(pom);
                    });
                    let div = document.querySelector(".divZaNarudzbinu");
                    prikazi(div);
                })
        })
}

function azurirajProizvodeUProdavnici(fun) {
    let imeProd = document.querySelector(".imeProdavnice").value;
    fetch("https://localhost:5001/ProdavnicaProizvod/PreuzmiImenaVeceOdNula/" + imeProd)
        .then(x => {
            x.json()
                .then(proizvodi => {
                    listaProizvodaUProdavnici.length = 0;
                    proizvodi.forEach(proizvod => {
                        let pom = new ProdavnicaProizvod(proizvod.id, proizvod.cena, proizvod.kolicina, proizvod.alijasProdavnice, proizvod.alijasProizvoda);
                        listaProizvodaUProdavnici.push(pom);
                    });
                    fun();
                })
        })
}

function ucitajDostavljace() {
    let imeProd = document.querySelector(".imeProdavnice").value;
    let idP = (listaProdavnica.find(x => x.naziv === imeProd)).id;
    fetch("https://localhost:5001/Dostavljac/PreuzmiDostavljace/" + idP)
        .then(x => {
            x.json()
                .then(dostavljaci => {
                    listaDostavljaca.length = 0;
                    dostavljaci.forEach(dostavljac => {
                        let pom = new Dostavljac(dostavljac.id, dostavljac.ime, dostavljac.prezime, dostavljac.godine, dostavljac.brTelefona, dostavljac.cenaUsluge, dostavljac.alijasProdavnice);
                        listaDostavljaca.push(pom);
                    });
                    console.log(listaDostavljaca);
                    upisiDostavljace();
                })
        })
}

function upisiDostavljace() {
    let p;
    let sel = document.querySelector(".imeDostavljaca");
    listaDostavljaca.forEach(dostavljac => {
        p = document.createElement("option");
        p.innerHTML = dostavljac.ime + " " + dostavljac.prezime + ", cena: " + dostavljac.cenaUsluge;
        p.value = dostavljac.id;
        sel.appendChild(p);
    });
}

function napraviGlavnuFormu() {
    let glavnaForma = document.createElement("div");
    glavnaForma.className = "glavnaForma";
    document.body.appendChild(glavnaForma);

    let goreForma = document.createElement("div");
    goreForma.className = "goreForma";
    goreForma.id = "goreForma";
    glavnaForma.appendChild(goreForma);

    let goreLevoForma = document.createElement("div");
    goreLevoForma.className = "goreLevoForma";
    goreLevoForma.id = "goreLevoForma";
    goreForma.appendChild(goreLevoForma);
    napraviGoreLevuFormu(goreLevoForma);

    let goreDesnoForma = document.createElement("div");
    goreDesnoForma.className = "goreDesnoForma";
    goreDesnoForma.id = "goreDesnoForma";
    goreForma.appendChild(goreDesnoForma);

    let doleForma = document.createElement("div");
    doleForma.className = "doleForma";
    doleForma.id = "doleForma";
    glavnaForma.appendChild(doleForma);

}

function napraviGoreLevuFormu(host) {
    let l = document.createElement("label");
    l.innerHTML = "Izaberite prodavnicu: ";
    host.appendChild(l);

    let sel = document.createElement("select");
    sel.className = "imeProdavnice";
    host.appendChild(sel);

    upisiProdavnice();

    let btn = document.createElement("button");
    btn.className = "btnPogledajKatalog";
    host.appendChild(btn);
    let txt = document.createTextNode("Pogledaj katalog");
    btn.appendChild(txt);

    let divZaNarudzbinu = document.createElement("div");
    divZaNarudzbinu.className = "divZaNarudzbinu";
    divZaNarudzbinu.id = "divZaNarudzbinu";
    host.appendChild(divZaNarudzbinu);

    btn.onclick = (ev) => ucitajProizvodeUProdavnici();
}

function prikazi(host) {
    obrisiFormu("goreDesnoForma");
    obrisiFormu("divZaNarudzbinu");
    let goreDesnoForma = document.querySelector(".goreDesnoForma");
    napraviGoreDesnuFormu(goreDesnoForma);
    prikazZaNarudzbu(host);
}

function napraviGoreDesnuFormu(host) {
    let pom;
    let slika;
    let labCena;
    let labKol;
    let btn;
    console.log(listaProizvodaUProdavnici);
    listaProizvodaUProdavnici.forEach(x => {
        let txt = document.createTextNode("+");
        pom = document.createElement("div");
        pom.className = "divProizvoda";
        host.appendChild(pom);
        slika = document.createElement("img");
        slika.className = "slike"
        slika.src = "../Slike/" + x.proizvod + ".jpg";
        slika.alt = x.proizvod + ".jpg";
        labCena = document.createElement("label");
        labKol = document.createElement("label");
        labCena.innerHTML = "Cena: " + x.cena + " dinara";
        labCena.value = x.cena;
        pom.appendChild(labCena);
        labKol.innerHTML = "Kolicina: " + x.kolicnaProizvoda;
        labKol.value = x.kolicina;
        labKol.id = "kolicina" + x.proizvod;
        pom.appendChild(labKol);
        pom.appendChild(slika);
        btn = document.createElement("button");
        btn.className = x.proizvod + "DugmeDodaj";
        btn.onclick = (ev) => dodajProizvodUNarudzbu(x.proizvod);
        btn.disabled = true;
        pom.appendChild(btn);
        btn.appendChild(txt);
        btn = document.createElement("button");
        btn.className = x.proizvod + "DugmeIzbaci";
        btn.onclick = (ev) => izbaciProizvodIzNarudzbe(x.proizvod);
        btn.disabled = true;
        pom.appendChild(btn);
        txt = document.createTextNode("-");
        btn.appendChild(txt);
    })
}

function dodajProizvodUNarudzbu(proizvod) {
    console.log(listaProizvodaUProdavnici);
    let pomPr = listaProizvodaUProdavnici.find(p => p.proizvod === proizvod);
    console.log(pomPr);
    let pom = listaProizvodaUNarudzbini.find(p => p.proizvod === proizvod);
    console.log(listaProizvodaUNarudzbini);
    if (pom == null) {
        console.log(proizvod);
        pom = new ProizvodNarudzbina(1, proizvod, pomPr.cena);
        listaProizvodaUNarudzbini.push(pom);
        let btn = document.querySelector("." + proizvod + "DugmeIzbaci");
        btn.disabled = false;
    }
    else {
        if (pomPr.kolicina === 0) {
            let btn = document.querySelector("." + proizvod + "DugmeDodaj");
            btn.disabled = true;
            pom.azurirajPostojeciProizvod(pom.kolicina + 1, pom.cena + pomPr.cena);
        }
        else {
            pom.azurirajPostojeciProizvod(pom.kolicina + 1, pom.cena + pomPr.cena);
        }
    }
    let forma = document.querySelector(".proizvodiUNarudzbi");
    obrisiFormu("proizvodiUNarudzbi");
    pom = document.createElement("label");
    console.log(listaProizvodaUNarudzbini);
    listaProizvodaUNarudzbini.forEach(x => {
        console.log("Proizvod: " + x.proizvod + ", kolicina: " + x.kolicina);
        pom.innerHTML += "Proizvod: " + x.proizvod + ", kolicina: " + x.kolicina + " " + x.cena + "<br>";
    })
    forma.appendChild(pom);
    let imeProd = document.querySelector(".imeProdavnice").value;
    fetch("https://localhost:5001/ProdavnicaProizvod/IzmeniProizvodUProdavnici/" + imeProd + "/" + proizvod + "/" + (-1), { method: "PUT" })
        .then(x => {
            if (x.ok) {
                azurirajProizvodeUProdavnici(function () { azurirajDesnuFormu() });
            }

        })
}

function izbaciProizvodIzNarudzbe(proizvod) {
    let pom = listaProizvodaUNarudzbini.find(p => p.proizvod === proizvod);
    let pomPr = listaProizvodaUProdavnici.find(p => p.proizvod === proizvod);
    console.log(listaProizvodaUNarudzbini);
    if (pom == null) {
        console.log("GRESKA");
    }
    else {
        console.log(pom.kolicina);
        if (pom.kolicina - 1 == 0) {
            let ind = listaProizvodaUNarudzbini.findIndex(p => p.proizvod === proizvod);
            listaProizvodaUNarudzbini.splice(ind, 1);
            let btn = document.querySelector("." + proizvod + "DugmeIzbaci");
            btn.disabled = true;
        }
        else {
            pom.azurirajPostojeciProizvod(pom.kolicina - 1, pom.cena - pomPr.cena);
        }
    }
    let forma = document.querySelector(".proizvodiUNarudzbi");
    obrisiFormu("proizvodiUNarudzbi");
    pom = document.createElement("label");
    listaProizvodaUNarudzbini.forEach(x => {
        console.log("Proizvod: " + x.proizvod + ", kolicina: " + x.kolicina);
        pom.innerHTML += "Proizvod: " + x.proizvod + ", kolicina: " + x.kolicina + " " + x.cena + "<br>";
    })
    forma.appendChild(pom);
    let imeProd = document.querySelector(".imeProdavnice").value;
    fetch("https://localhost:5001/ProdavnicaProizvod/IzmeniProizvodUProdavnici/" + imeProd + "/" + proizvod + "/" + 1, { method: "PUT" })
        .then(x => {
            if (x.ok) {
                azurirajProizvodeUProdavnici(function () { azurirajDesnuFormu() });
            }

        })
}

function azurirajDesnuFormu() {
    let imeProd = document.querySelector(".imeProdavnice").value;
    let pom;
    let ime;
    console.log(listaProizvodaUProdavnici);
    listaProizvodaUProdavnici.forEach(x => {
        ime = "kolicina" + x.proizvod;
        pom = document.querySelector("#" + ime);
        //pom = document.getElementById(ime);
        if (x.kolicnaProizvoda == 0) {
            document.querySelector("." + x.proizvod + "DugmeDodaj").disabled = true;
        }
        else {
            document.querySelector("." + x.proizvod + "DugmeDodaj").disabled = false;
        }
        pom.innerHTML = "Kolicina: " + x.kolicnaProizvoda;
        pom.value = x.kolicnaProizvoda;
    })
}

function prikazZaNarudzbu(host) {
    let pom;
    let pomLab;
    pomLab = document.createElement("label");
    pomLab.innerHTML = "Adresa";
    pom = document.createElement("input");
    pom.type = "text";
    pom.className = "adresa";
    pom.placeholder = "Adresa";
    host.appendChild(pomLab);
    host.appendChild(pom);
    pomLab = document.createElement("label");
    pomLab.innerHTML = "Broj telefona";
    pom = document.createElement("input");
    pom.type = "text";
    pom.className = "brTelefona";
    pom.placeholder = "+381";
    host.appendChild(pomLab);
    host.appendChild(pom);
    pomLab = document.createElement("label");
    pomLab.innerHTML = "Izaberi dostavljaca:";
    pom = document.createElement("select");
    pom.className = "imeDostavljaca";
    host.appendChild(pomLab);
    host.appendChild(pom);
    ucitajDostavljace();
    let btn = document.createElement("button");
    btn.className = "btnKreirajNarudzbu";
    host.appendChild(btn);
    let txt = document.createTextNode("Kreiraj narudzbinu");
    btn.appendChild(txt);
    btn.onclick = (ev) => {
        let adresa = document.querySelector(".adresa").value;
        let brTelefona = document.querySelector(".brTelefona").value;
        let imeProd = document.querySelector(".imeProdavnice").value;
        let idD = document.querySelector(".imeDostavljaca").value;
        let btn;
        let neValidan = false;
        let phoneno = "^\\s*\\+?\\s*([0-9][\\s-]*){9,}$";
        if (brTelefona.match(phoneno)) {
            neValidan = false;
        }
        else {
            neValidan = true;
        }
        if ((adresa === "" && brTelefona === "") || neValidan) {
            alert("Ne validna adresa ili broj telefona");
        }
        else {
            fetch("https://localhost:5001/Narudzbina/DodajNarudzbinu/" + adresa + "/" + brTelefona + "/" + imeProd + "/" + idD, { method: "POST" })
                .then(x => {
                    if (x.ok) {
                        listaProizvodaUProdavnici.forEach(x => {
                            if (x.kolicnaProizvoda > 0) {
                                btn = document.querySelector("." + x.proizvod + "DugmeDodaj");
                                btn.disabled = false;
                            }
                            else {
                                if (x.kolicnaProizvoda == 0) {
                                    btn = document.querySelector("." + x.proizvod + "DugmeDodaj");
                                    btn.disabled = true;
                                }
                            }
                        })
                        btn = document.querySelector(".btnPotvrdiNarudzbu");
                        btn.disabled = false;
                    }
                    else {
                        alert("Narudzbina nije moguca.Proverite podatke");
                    }
                })
        }
    }
    pom = document.createElement("div");
    pom.className = "proizvodiUNarudzbi";
    pom.id = "proizvodiUNarudzbi";
    host.appendChild(pom);
    btn = document.createElement("button");
    btn.className = "btnPotvrdiNarudzbu";
    host.appendChild(btn);
    txt = document.createTextNode("Potvrdi narudzbinu");
    btn.appendChild(txt);
    btn.disabled = true;
    btn.onclick = (ev) => potvrdiNarudzbinu();
}

function potvrdiNarudzbinu() {
    vratiNarudzbinu();

    let adresa = document.querySelector(".adresa").value;
    let brtelefona = document.querySelector(".brTelefona").value;
    let cena = 0;
    listaProizvodaUNarudzbini.forEach(x => {
        cena += x.cena;
        fetch("https://localhost:5001/ProizvodNarudzbina/DodajProizvodUNarudzbinu/" + adresa + "/" + brtelefona + "/" + x.proizvod + "/" + x.kolicina, { method: "POST" })
            .then(x => {
                if (x.ok) {
                    console.log("Uspesno dodat");
                }
            })
    })
    let dos = document.querySelector(".imeDostavljaca").value;
    console.log(dos);
    let dosta = listaDostavljaca.find(x => x.id == dos);
    cena += dosta.cenaUsluge;
    console.log(cena);
    alert("Vasa narudzbina je prihvacena, cena iznosi: " + cena + ".Dostavljac " + dosta.ime + " " + dosta.prezime + " broj telefona " + dosta.brTelefona);
    location.reload();
}

function crtajGrafik() {
    let doleForma = document.querySelector(".doleForma");
    listaProdavnica.forEach(i => {
        let prodavnicaDiv = document.createElement("div");
        prodavnicaDiv.className = "prodavnicaDiv";
        doleForma.appendChild(prodavnicaDiv);
        let tekstDiv = document.createElement("div");
        tekstDiv.className = "tekstDiv";
        prodavnicaDiv.appendChild(tekstDiv);
        let grafikaDiv = document.createElement("div");
        grafikaDiv.className = "grafikaDiv";
        prodavnicaDiv.appendChild(grafikaDiv);
        let brojac = 0;
        let crveniDiv;
        listaNarudzbina.forEach(j => {
            crveniDiv = document.createElement("div");
            crveniDiv.className = "crveniDiv";
            if (j.idProdavnice === i.id) {
                grafikaDiv.appendChild(crveniDiv);
                brojac++;
            }
        })
        let txtNaziv = document.createTextNode(i.naziv);
        let br = document.createElement("br");
        let txtKol = document.createTextNode("Broj narudzbina:" + brojac);
        tekstDiv.appendChild(txtNaziv);
        tekstDiv.appendChild(br);
        tekstDiv.appendChild(txtKol);
    })
}

function obrisiFormu(forma, fun) {
    let formaBrisanje = document.querySelector("#" + forma);
    while (formaBrisanje.firstChild) {
        formaBrisanje.removeChild(formaBrisanje.firstChild);
    }
    if (fun != null) {
        fun();
    }
}