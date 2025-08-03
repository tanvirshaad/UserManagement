// Global variables
let selectedUsers = new Set();
let currentSortColumn = 'lastLogin';
let currentSortDirection = 'desc';

// Initialize the dashboard
document.addEventListener('DOMContentLoaded', function() {
    initializeTooltips();
    initializeEventListeners();
    updateSelectedCount();
});

// Initialize Bootstrap tooltips
function initializeTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Initialize event listeners
function initializeEventListeners() {
    // Select all checkbox
    document.getElementById('selectAll').addEventListener('change', function() {
        const isChecked = this.checked;
        const checkboxes = document.querySelectorAll('.user-checkbox');
        checkboxes.forEach(checkbox => {
            checkbox.checked = isChecked;
            const userId = parseInt(checkbox.value);
            if (isChecked) {
                selectedUsers.add(userId);
            } else {
                selectedUsers.delete(userId);
            }
        });
        updateSelectedCount();
        updateToolbarButtons();
    });

    // Individual user checkboxes
    document.querySelectorAll('.user-checkbox').forEach(checkbox => {
        checkbox.addEventListener('change', function() {
            const userId = parseInt(this.value);
            if (this.checked) {
                selectedUsers.add(userId);
            } else {
                selectedUsers.delete(userId);
            }
            updateSelectedCount();
            updateToolbarButtons();
            updateSelectAllCheckbox();
        });
    });

    // Toolbar buttons
    document.getElementById('blockBtn').addEventListener('click', function() {
        performAction('block');
    });

    document.getElementById('unblockBtn').addEventListener('click', function() {
        performAction('unblock');
    });

    document.getElementById('deleteBtn').addEventListener('click', function() {
        performAction('delete');
    });

    // Filter input
    document.getElementById('filterInput').addEventListener('input', function() {
        filterTable(this.value);
    });

    // Confirm delete button
    document.getElementById('confirmDeleteBtn').addEventListener('click', function() {
        if (pendingDeleteUserIds.length > 0) {
            executeAction('delete', pendingDeleteUserIds);
            // Hide the modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('deleteConfirmModal'));
            modal.hide();
        }
    });
}

// Update selected count display
function updateSelectedCount() {
    const count = selectedUsers.size;
    document.getElementById('selectedCount').textContent = count;
}

// Update toolbar buttons state
function updateToolbarButtons() {
    const hasSelection = selectedUsers.size > 0;
    const buttons = ['blockBtn', 'unblockBtn', 'deleteBtn'];
    
    buttons.forEach(btnId => {
        const btn = document.getElementById(btnId);
        if (hasSelection) {
            btn.disabled = false;
            btn.classList.remove('disabled');
        } else {
            btn.disabled = true;
            btn.classList.add('disabled');
        }
    });
}

// Update select all checkbox state
function updateSelectAllCheckbox() {
    const checkboxes = document.querySelectorAll('.user-checkbox');
    const checkedCheckboxes = document.querySelectorAll('.user-checkbox:checked');
    const selectAllCheckbox = document.getElementById('selectAll');
    
    if (checkedCheckboxes.length === 0) {
        selectAllCheckbox.checked = false;
        selectAllCheckbox.indeterminate = false;
    } else if (checkedCheckboxes.length === checkboxes.length) {
        selectAllCheckbox.checked = true;
        selectAllCheckbox.indeterminate = false;
    } else {
        selectAllCheckbox.checked = false;
        selectAllCheckbox.indeterminate = true;
    }
}

// Filter table rows
function filterTable(filterValue) {
    const rows = document.querySelectorAll('#userTableBody tr');
    const filterLower = filterValue.toLowerCase();
    
    rows.forEach(row => {
        const name = row.querySelector('td:nth-child(2)').textContent.toLowerCase();
        const email = row.querySelector('td:nth-child(3)').textContent.toLowerCase();
        
        if (name.includes(filterLower) || email.includes(filterLower)) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
}

// Sort table
function sortTable(column) {
    const tbody = document.getElementById('userTableBody');
    const rows = Array.from(tbody.querySelectorAll('tr'));
    
    // Update sort direction
    if (currentSortColumn === column) {
        currentSortDirection = currentSortDirection === 'asc' ? 'desc' : 'asc';
    } else {
        currentSortColumn = column;
        currentSortDirection = 'asc';
    }
    
    // Update sort icons
    updateSortIcons(column);
    
    // Sort rows
    rows.sort((a, b) => {
        let aValue, bValue;
        
        switch (column) {
            case 'name':
                aValue = a.querySelector('td:nth-child(2) .fw-bold').textContent.toLowerCase();
                bValue = b.querySelector('td:nth-child(2) .fw-bold').textContent.toLowerCase();
                break;
            case 'email':
                aValue = a.querySelector('td:nth-child(3)').textContent.toLowerCase();
                bValue = b.querySelector('td:nth-child(3)').textContent.toLowerCase();
                break;
            case 'lastLogin':
                aValue = a.querySelector('td:nth-child(4) .last-login-text').textContent.toLowerCase();
                bValue = b.querySelector('td:nth-child(4) .last-login-text').textContent.toLowerCase();
                break;
            default:
                return 0;
        }
        
        if (currentSortDirection === 'asc') {
            return aValue.localeCompare(bValue);
        } else {
            return bValue.localeCompare(aValue);
        }
    });
    
    // Re-append sorted rows
    rows.forEach(row => tbody.appendChild(row));
}

// Update sort icons
function updateSortIcons(activeColumn) {
    const icons = ['nameSortIcon', 'emailSortIcon', 'lastLoginSortIcon'];
    const directions = ['asc', 'desc'];
    
    icons.forEach(iconId => {
        const icon = document.getElementById(iconId);
        const column = iconId.replace('SortIcon', '');
        
        if (column === activeColumn) {
            icon.className = `bi bi-chevron-${currentSortDirection === 'asc' ? 'up' : 'down'}`;
        } else {
            icon.className = 'bi bi-chevron-down';
        }
    });
}

// Global variables for modal
let pendingDeleteUserIds = [];

// Perform action (block, unblock, delete)
function performAction(action) {
    if (selectedUsers.size === 0) {
        showAlert('Please select at least one user.', 'warning');
        return;
    }
    
    const userIds = Array.from(selectedUsers);
    const actionText = action.charAt(0).toUpperCase() + action.slice(1);
    
    if (action === 'delete') {
        // Show delete confirmation modal
        showDeleteConfirmationModal(userIds);
        return;
    }
    
    // For block/unblock actions, proceed directly
    executeAction(action, userIds);
}

// Show delete confirmation modal
function showDeleteConfirmationModal(userIds) {
    pendingDeleteUserIds = userIds;
    document.getElementById('deleteUserCount').textContent = userIds.length;
    
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    modal.show();
}

// Execute the actual action (block, unblock, delete)
function executeAction(action, userIds) {
    const actionText = action.charAt(0).toUpperCase() + action.slice(1);
    
    // Disable buttons during request
    const buttons = ['blockBtn', 'unblockBtn', 'deleteBtn'];
    buttons.forEach(btnId => {
        document.getElementById(btnId).disabled = true;
    });
    
    // Make API request
    fetch(`/Home/${actionText}Users`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }
        return response.json();
    })
    .then(data => {
        if (data.redirect) {
            if (data.error === 'account_blocked') {
                window.location.href = '/Account/Login?error=account_blocked';
            } else {
                window.location.href = '/Account/Login?error=authentication_required';
            }
            return;
        }
        
        if (data.success) {
            showAlert(data.message, 'success');
            // Refresh the page to show updated data
            setTimeout(() => {
                window.location.reload();
            }, 1500);
        } else {
            showAlert(data.message || `Failed to ${action} users.`, 'danger');
        }
    })
    .catch(error => {
        console.error('Error:', error);
        if (error.message.includes('401') || error.message.includes('403')) {
            showAlert('Your account has been blocked or you are not authorized to perform this action.', 'danger');
            setTimeout(() => {
                window.location.href = '/Account/Login?error=account_blocked';
            }, 2000);
        } else {
            showAlert(`An error occurred while trying to ${action} users: ${error.message}`, 'danger');
        }
    })
    .finally(() => {
        // Re-enable buttons
        buttons.forEach(btnId => {
            document.getElementById(btnId).disabled = false;
        });
    });
}

// Show alert message
function showAlert(message, type) {
    const alertContainer = document.getElementById('alert-container');
    const alertId = 'alert-' + Date.now();
    
    const alertHtml = `
        <div id="${alertId}" class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    
    alertContainer.innerHTML = alertHtml;
    
    // Auto-dismiss after 5 seconds
    setTimeout(() => {
        const alert = document.getElementById(alertId);
        if (alert) {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }
    }, 5000);
} 