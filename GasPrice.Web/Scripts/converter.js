function hexToDec(value) {
    const r = parseInt(value, 16);

    if (isNaN(r)) { throw "Invalid value"; }
    return r;
}

function decToHex(value) {
    value = parseInt(value);
    if (isNaN(value)) { throw "Invalid value"; }
    if (value < 0) {
        value = 0xFFFFFFFF + value + 1;
    }
    return "0x" + value.toString(16).toUpperCase();
}


BigNumber.config({ EXPONENTIAL_AT: 50 });
BigNumber.config({ ERRORS: false });

function calculateUSD() {
    const total = parseFloat($("#usd").data("rate"));
    document.getElementById("usd").value = (parseFloat(document.getElementById("rbtc").value) * total).toFixed(3);
}

function calculate(el) {
    $("#rbtcconvert input").removeClass("error");
    if(isNaN(el.value))
    {
        $(el).addClass("error");
    }

    calculaterbtc(el.id, convertTorbtc(el));

    calculateUSD();
}

function calculaterbtc(id, v) {
    if (id != "wei") document.getElementById("wei").value = v.times(new BigNumber(1000000000000000000)).toString();
    if (id != "kwei") document.getElementById("kwei").value = v.times(new BigNumber(1000000000000000)).toString();
    if (id != "mwei") document.getElementById("mwei").value = v.times(new BigNumber(1000000000000)).toString();
    if (id != "gwei") document.getElementById("gwei").value = v.times(new BigNumber(1000000000)).toString();
    if (id != "szabo") document.getElementById("szabo").value = v.times(new BigNumber(1000000)).toString();
    if (id != "finney") document.getElementById("finney").value = v.times(new BigNumber(1000)).toString();
    if (id != "rbtc") document.getElementById("rbtc").value = v.times(new BigNumber(1)).toString();
    if (id != "krbtc") document.getElementById("krbtc").value = v.times(new BigNumber(0.001)).toString();
    if (id != "mrbtc") document.getElementById("mrbtc").value = v.times(new BigNumber(0.000001)).toString();
    if (id != "grbtc") document.getElementById("grbtc").value = v.times(new BigNumber(0.000000001)).toString();
    if (id != "trbtc") document.getElementById("trbtc").value = v.times(new BigNumber(0.000000000001)).toString();
}

function convertTorbtc(el) {
    var id = el.id;
    var value = new BigNumber(el.value);
    if (isNaN(value)) {
        $(el).addClass("error");
        return new BigNumber(0);
    }
    switch (id) {
        case "wei":
            value = value.times(new BigNumber(0.000000000000000001));
            break;
        case "kwei":
            value = value.times(new BigNumber(0.000000000000001));
            break;
        case "mwei":
            value = value.times(new BigNumber(0.000000000001));
            break;
        case "gwei":
            value = value.times(new BigNumber(0.000000001));
            break;
        case "szabo":
            value = value.times(new BigNumber(0.000001));
            break;
        case "finney":
            value = value.times(new BigNumber(0.001));
            break;
        case "rbtc":
            value = value.times(new BigNumber(1));
            break;
        case "krbtc":
            value = value.times(new BigNumber(1000));
            break;
        case "mrbtc":
            value = value.times(new BigNumber(1000000));
            break;
        case "grbtc":
            value = value.times(new BigNumber(1000000000));
            break;
        case "trbtc":
            value = value.times(new BigNumber(1000000000000));
            break;
        default:
            break;
    }
    if (isNaN(value)) return new BigNumber(0);
    return value;
}

function init() {

    const $source = document.querySelectorAll('#numberconvert input');

    const typeHandler = function (e) {
        $($source).removeClass("error");
        const el = $(e.target);
        const fn = window[el.data("function")];
        const destination = $(el.data("result"));
        try {
            const r = fn(el.val());
            destination.val(r);
        } catch (e) {
            el.addClass("error");
            destination.val("");
        }
    }

    $source.forEach(function (s) {
        s.addEventListener('input', typeHandler); // register for oninput
        s.addEventListener('propertychange', typeHandler); // for IE8
    });

    calculaterbtc("rbtc", new BigNumber(1));
    calculateUSD();
}

init();