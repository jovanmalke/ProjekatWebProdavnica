napraviGlavnuFormu();

function napraviGlavnuFormu() {
    let glavniDiv = document.createElement("div");
    glavniDiv.className = "glavniDiv";
    document.body.appendChild(glavniDiv);
    let dugmiciDiv = document.createElement("div");
    dugmiciDiv.className = "dugmiciDiv";
    glavniDiv.appendChild(dugmiciDiv);
    let link = document.createElement("a");
    link.href = "izaberi.html";
    dugmiciDiv.appendChild(link);
    let btn = document.createElement("button");
    let txt = document.createTextNode("Napravi narudzbinu");
    btn.className = "dugme";
    btn.appendChild(txt);
    link.appendChild(btn);
    link = document.createElement("a");
    link.href = "dodaj.html";
    dugmiciDiv.appendChild(link);
    btn = document.createElement("button");
    btn.className = "dugme";
    txt = document.createTextNode("Upravljaj prodavnicom");
    btn.appendChild(txt);
    link.appendChild(btn);
}
