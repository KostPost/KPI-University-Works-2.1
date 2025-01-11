using System.Data;
using Npgsql;

namespace Laba3;

public partial class MainForm : Form
{
    private readonly DataService dataService;

    public MainForm()
    {
        InitializeComponent();
        dataService = new DataService();

        try
        {
            dataService.InitializeDatabase();
            lblStatus.Text = "Connected to database successfully";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Database initialization failed: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = "Database connection failed";
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);
        dataService?.Dispose();
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (!int.TryParse(txtKey.Text, out int key))
            {
                MessageBox.Show("Please enter a valid integer key");
                return;
            }

            var record = dataService.Search(key);
            if (record != null)
            {
                txtData.Text = record.Data;
                lblStatus.Text = "Record found";
            }
            else
            {
                lblStatus.Text = "Record not found";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error during search: {ex.Message}");
        }
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (!int.TryParse(txtKey.Text, out int key))
            {
                MessageBox.Show("Please enter a valid integer key");
                return;
            }

            string data = txtData.Text;
            if (string.IsNullOrEmpty(data))
            {
                MessageBox.Show("Please enter data");
                return;
            }

            dataService.Insert(key, data);
            lblStatus.Text = "Record added successfully";
            RefreshDataGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error adding record: {ex.Message}");
        }
    }

    private void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (!int.TryParse(txtKey.Text, out int key))
            {
                MessageBox.Show("Please enter a valid integer key");
                return;
            }

            string newData = txtData.Text;
            if (string.IsNullOrEmpty(newData))
            {
                MessageBox.Show("Please enter data");
                return;
            }

            dataService.Update(key, newData);
            lblStatus.Text = "Record updated successfully";
            RefreshDataGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating record: {ex.Message}");
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (!int.TryParse(txtKey.Text, out int key))
            {
                MessageBox.Show("Please enter a valid integer key");
                return;
            }

            dataService.Delete(key);
            lblStatus.Text = "Record deleted successfully";
            RefreshDataGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error deleting record: {ex.Message}");
        }
    }

    private void BtnGenerateData_Click(object sender, EventArgs e)
    {
        try
        {
            dataService.GenerateRandomData(10000);
            lblStatus.Text = "Generated 10,000 random records";
            RefreshDataGrid();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error generating data: {ex.Message}");
        }
    }

    private void BtnMeasure_Click(object sender, EventArgs e)
    {
        try
        {
            double avgComparisons = dataService.MeasureAverageComparisons();
            lblStatus.Text = $"Average search time: {avgComparisons:F2} ms";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error measuring performance: {ex.Message}");
        }
    }

    private void RefreshDataGrid()
    {
        try
        {
            dataGridView.DataSource = dataService.GetAllRecords();

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            lblStatus.Text = $"Data refreshed successfully. Showing {dataGridView.RowCount} records.";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to refresh data grid", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblStatus.Text = $"Failed to refresh data: {ex.Message}";
        }
    }
}