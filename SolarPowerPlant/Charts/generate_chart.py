import matplotlib.pyplot as plt
import pandas as pd
import sys
import psycopg2

def generate_line_chart(db_connection_string, power_plant_id, date, file_name):
    
    connection = psycopg2.connect(db_connection_string)
    
    query = f'SELECT "Timestamp", "ProductionValue" FROM "ProductionData" WHERE "Type" = 0 and "PowerPlantId" = \'{power_plant_id}\' AND Date("Timestamp") = \'{date}\''
    
    df = pd.read_sql_query(query, connection)
    
    plt.plot(df['Timestamp'], df['ProductionValue'], label='Power Production')
    plt.xlabel('Timestamp')
    plt.ylabel('Production Value')  
    plt.title(f'Solar Power Plant Production Data - {date}')
    plt.legend()
    
    plt.savefig(file_name)

if __name__ == "__main__":
    if len(sys.argv) != 5:
        print("Usage: python generate_chart.py <db_connection_string> <power_plant_id> <date> <file_name>")
        sys.exit(1)
    
    db_connection_string = sys.argv[1].replace("localhost", "database")
    power_plant_id = sys.argv[2]
    date = sys.argv[3]
    file_name = sys.argv[4]

    generate_line_chart(db_connection_string, power_plant_id, date, file_name)
