const sampleCsv = `Date,Description,Amount,Currency
2026-06-03,SPOTIFY PREMIUM,-59.99,TRY
2026-06-05,MIGROS SANAL MARKET,-845.20,TRY
2026-06-07,MAAS ODEMESI,75000,TRY
2026-06-09,SHELL ISTASYON,-1850.00,TRY
2026-06-12,NETFLIX.COM,-149.99,TRY
2026-07-03,SPOTIFY PREMIUM,-59.99,TRY
2026-07-05,MIGROS SANAL MARKET,-912.10,TRY
2026-07-08,MAAS ODEMESI,76000,TRY
2026-07-10,TRENDYOL MARKET,-420.50,TRY
2026-07-12,NETFLIX.COM,-149.99,TRY
2026-07-13,UBER TRIP,-280.00,TRY
2026-07-18,TURKCELL FATURA,-530.45,TRY
2026-07-22,STARBUCKS,-210.00,TRY`;

const currency = new Intl.NumberFormat("tr-TR", {
    style: "currency",
    currency: "TRY",
    maximumFractionDigits: 2
});

document.getElementById("load-sample").addEventListener("click", () => {
    document.getElementById("csv-content").value = sampleCsv;
});

document.getElementById("preview-button").addEventListener("click", previewImport);
document.getElementById("import-button").addEventListener("click", runImport);
document.getElementById("refresh-dashboard").addEventListener("click", refreshDashboard);
document.getElementById("refresh-transactions").addEventListener("click", refreshTransactions);
document.getElementById("refresh-report").addEventListener("click", refreshReport);

async function fetchJson(url, options) {
    const response = await fetch(url, options);
    if (!response.ok) {
        const body = await response.text();
        throw new Error(body || "Istek basarisiz oldu.");
    }

    return response.json();
}

async function loadHealth() {
    const health = await fetchJson("/api/system/health");
    document.getElementById("health-status").textContent = health.status;
    document.getElementById("health-time").textContent = new Date(health.utcTime).toLocaleString("tr-TR");
}

async function previewImport() {
    const payload = getImportPayload();
    const preview = await fetchJson("/api/imports/preview", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    setFeedback(`Onizleme hazir. ${preview.totalRows} satir bulundu.`);
    renderPreviewRows(preview.previewRows || []);
}

async function runImport() {
    const payload = getImportPayload();
    const result = await fetchJson("/api/imports", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    const warningText = (result.warnings || []).length > 0
        ? ` Uyarilar: ${(result.warnings || []).join(" | ")}`
        : "";

    setFeedback(`${result.importedTransactions} islem ice aktarıldi.${warningText}`);
    await refreshAll();
}

function getImportPayload() {
    return {
        fileName: document.getElementById("file-name").value || "upload.csv",
        content: document.getElementById("csv-content").value
    };
}

function setFeedback(text) {
    document.getElementById("import-feedback").textContent = text;
}

function renderPreviewRows(rows) {
    const tbody = document.querySelector("#preview-table tbody");
    tbody.innerHTML = rows.map(row => `
        <tr>
            <td>${row.transactionDate || ""}</td>
            <td>${escapeHtml(row.description)}</td>
            <td>${currency.format(row.amount)}</td>
            <td>${row.currency}</td>
        </tr>
    `).join("");
}

async function refreshDashboard() {
    const year = document.getElementById("year-input").value;
    const month = document.getElementById("month-input").value;

    const [summary, categories, merchants, subscriptions] = await Promise.all([
        fetchJson(`/api/dashboard/summary?year=${year}&month=${month}`),
        fetchJson(`/api/dashboard/category-breakdown?year=${year}&month=${month}`),
        fetchJson(`/api/dashboard/top-merchants?year=${year}&month=${month}&take=5`),
        fetchJson("/api/subscriptions")
    ]);

    document.getElementById("metric-expense").textContent = currency.format(summary.totalExpense || 0);
    document.getElementById("metric-income").textContent = currency.format(summary.totalIncome || 0);
    document.getElementById("metric-category").textContent = summary.topCategory || "-";
    document.getElementById("metric-subscriptions").textContent = `${summary.subscriptionCount || 0} adet / ${currency.format(summary.subscriptionTotal || 0)}`;

    document.getElementById("category-list").innerHTML = categories.map(item => `
        <li><strong>${escapeHtml(item.category)}</strong><br>${currency.format(item.amount)} • %${item.percentage}</li>
    `).join("") || "<li>Veri bekleniyor.</li>";

    document.getElementById("merchant-list").innerHTML = merchants.map(item => `
        <li><strong>${escapeHtml(item.merchantName)}</strong><br>${currency.format(item.amount)} • ${item.transactionCount} islem</li>
    `).join("") || "<li>Veri bekleniyor.</li>";

    document.getElementById("subscription-list").innerHTML = subscriptions.map(item => `
        <li><strong>${escapeHtml(item.merchantName)}</strong><br>${currency.format(item.estimatedMonthlyAmount)} • ${item.paymentCount} tekrar • guven ${item.confidence}</li>
    `).join("") || "<li>Henuz abonelik adayi yok.</li>";
}

async function refreshTransactions() {
    const year = document.getElementById("year-input").value;
    const month = document.getElementById("month-input").value;
    const result = await fetchJson(`/api/transactions?year=${year}&month=${month}`);
    const tbody = document.querySelector("#transactions-table tbody");

    tbody.innerHTML = (result.items || []).map(item => `
        <tr>
            <td>${item.date}</td>
            <td>${escapeHtml(item.merchantName)}</td>
            <td>${escapeHtml(item.category)}</td>
            <td>${currency.format(item.amount)}</td>
            <td>${item.direction === 0 ? "Gider" : "Gelir"}</td>
            <td>${item.confidence}</td>
        </tr>
    `).join("");
}

async function refreshReport() {
    const year = document.getElementById("year-input").value;
    const month = document.getElementById("month-input").value;
    const report = await fetchJson(`/api/dashboard/report?year=${year}&month=${month}`);

    document.getElementById("report-title").textContent = report.title || "Aylik Rapor";
    document.getElementById("report-summary").textContent = report.summary || "";
    document.getElementById("report-points").innerHTML = (report.highlights || []).map(item => `
        <li>${escapeHtml(item)}</li>
    `).join("");
}

async function refreshAll() {
    await Promise.all([
        refreshDashboard(),
        refreshTransactions(),
        refreshReport()
    ]);
}

function escapeHtml(value) {
    return value
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;");
}

document.getElementById("csv-content").value = sampleCsv;
loadHealth().then(refreshAll).catch(error => {
    setFeedback(error.message);
});
