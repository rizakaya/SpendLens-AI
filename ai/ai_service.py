import json
from http.server import BaseHTTPRequestHandler, HTTPServer
from urllib.parse import urlparse


def classify_transaction(description: str, amount: float, currency: str) -> dict:
    normalized = description.strip().upper()
    keyword_map = [
        (["SPOTIFY", "NETFLIX", "YOUTUBE", "APPLE.COM/BILL"], "Subscription", True),
        (["MIGROS", "A101", "BIM", "MARKET"], "Groceries", False),
        (["TRENDYOL", "HEPSIBURADA", "AMAZON"], "Shopping", False),
        (["SHELL", "OPET", "BP"], "Fuel", False),
        (["UBER", "BITAKSI", "METRO"], "Transport", False),
        (["STARBUCKS", "CAFE", "RESTAURANT"], "Dining", False),
        (["TURKCELL", "VODAFONE", "FATURA", "ELEKTRIK", "SU"], "Bills", True),
        (["MAAS", "SALARY"], "Income", False),
    ]

    merchant_name = description.strip().split("*")[-1].split("/")[0].strip() or "Unknown"
    category = "Other"
    subscription = False
    confidence = 0.72

    for keywords, matched_category, is_subscription in keyword_map:
        if any(keyword in normalized for keyword in keywords):
            category = matched_category
            subscription = is_subscription
            confidence = 0.92 if is_subscription else 0.86
            merchant_name = keywords[0].title() if len(description.strip()) == 0 else merchant_name.title()
            break

    if amount > 0:
        category = "Income"
        subscription = False
        confidence = 0.98

    merchant_name = clean_merchant(description)
    normalized_description = f"{merchant_name} transaction categorized as {category.lower()}."

    return {
        "merchantName": merchant_name,
        "normalizedDescription": normalized_description,
        "category": category,
        "isSubscriptionCandidate": subscription,
        "confidence": confidence,
    }


def clean_merchant(description: str) -> str:
    text = description.strip().replace("*", " ").replace("/", " ")
    parts = [part for part in text.split() if part]
    if not parts:
        return "Unknown"

    merchant = " ".join(parts[:2]).title()
    replacements = {
        "Maas Odemesi": "Salary Payment",
        "Migros Sanal": "Migros",
        "Spotify Premium": "Spotify",
        "Netflix.Com": "Netflix",
        "Turkcell Fatura": "Turkcell",
    }
    return replacements.get(merchant, merchant)


def build_report(payload: dict) -> dict:
    payload = normalize_keys(payload)
    summary = payload.get("summary", {})
    top_categories = payload.get("topcategories", [])
    subscriptions = payload.get("subscriptions", [])
    year = summary.get("year", 0)
    month = summary.get("month", 0)
    top_category = summary.get("topcategory", "N/A")
    total_expense = summary.get("totalexpense", 0)
    total_income = summary.get("totalincome", 0)
    subscription_total = summary.get("subscriptiontotal", 0)

    category_line = "No spending categories were detected yet."
    if top_categories:
        category_line = (
            f"Top category is {top_categories[0].get('category', 'N/A')} "
            f"with {top_categories[0].get('percentage', 0)}% of total expense."
        )

    subscription_line = "No recurring subscriptions were detected."
    if subscriptions:
        subscription_line = f"{len(subscriptions)} recurring subscription candidates were found, totaling about {subscription_total:.2f} TRY per month."

    return {
        "title": f"{year}-{month:02d} spending report",
        "summary": (
            f"In {year}-{month:02d}, total expense was {total_expense:.2f} TRY and income was {total_income:.2f} TRY. "
            f"The leading expense category was {top_category}. {subscription_line}"
        ),
        "highlights": [
            category_line,
            subscription_line,
            "Keep an eye on merchants with repeat charges and review shopping spikes for savings opportunities.",
        ],
    }


class RequestHandler(BaseHTTPRequestHandler):
    def do_GET(self):
        if urlparse(self.path).path == "/health":
            return self.send_json(200, {"status": "ok", "service": "spendlens-ai-service"})

        self.send_json(404, {"error": "Not found"})

    def do_POST(self):
        path = urlparse(self.path).path
        content_length = int(self.headers.get("Content-Length", "0"))
        raw_body = self.rfile.read(content_length).decode("utf-8") if content_length else "{}"
        payload = json.loads(raw_body)

        if path == "/api/enrich":
            payload = normalize_keys(payload)
            description = payload.get("description", "")
            amount = float(payload.get("amount", 0))
            currency = payload.get("currency", "TRY")
            return self.send_json(200, classify_transaction(description, amount, currency))

        if path == "/api/report":
            return self.send_json(200, build_report(payload))

        self.send_json(404, {"error": "Not found"})

    def log_message(self, format, *args):
        return

    def send_json(self, status_code: int, payload: dict):
        body = json.dumps(payload).encode("utf-8")
        self.send_response(status_code)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)


def normalize_keys(payload: dict) -> dict:
    normalized = {}
    for key, value in payload.items():
        lowered = key.lower()
        if isinstance(value, dict):
            normalized[lowered] = normalize_keys(value)
        elif isinstance(value, list):
            normalized[lowered] = [
                normalize_keys(item) if isinstance(item, dict) else item
                for item in value
            ]
        else:
            normalized[lowered] = value

    return normalized


if __name__ == "__main__":
    server = HTTPServer(("0.0.0.0", 8081), RequestHandler)
    server.serve_forever()
