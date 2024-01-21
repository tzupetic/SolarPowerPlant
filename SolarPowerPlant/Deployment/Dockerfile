FROM python:3.8

WORKDIR /app

COPY Deployment/requirements.txt .

RUN pip install --no-cache-dir -r requirements.txt

COPY Scripts/generate_chart.py .

CMD ["tail", "-f", "/dev/null"]