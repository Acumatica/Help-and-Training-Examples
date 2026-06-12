import { EventAggregator } from "aurelia-event-aggregator";
import { bindable, Disposable } from "aurelia-framework";
import { autoinject, IScreenApiResult } from "client-controls";

export class RecordInsightsPanelCustomElement {
    @bindable title: string = "Record Insights Panel";
    @bindable status: string = "";
    @bindable warningMessage: string = "";
    @bindable details: string = "";
    @bindable collapsed: boolean = false;
    @bindable accent: string = "neutral";

    screenCaption: string = "";
    lastScreenUpdate: string = "";
    hasWarning: boolean = false;

    @autoinject
    public eventAggregator!: EventAggregator;

    private subscriptions: Disposable[] = [];

    attached() {
        this.hasWarning = !!this.warningMessage?.trim();

        this.subscriptions.push(
            this.eventAggregator.subscribe(
                "screen-updated",
                (screenData: IScreenApiResult) => {
                    this.screenCaption = `Screen ${screenData.screenID}`;
                    this.lastScreenUpdate = new Date().toLocaleTimeString();
                }
            )
        );
    }

    detached() {
        this.subscriptions.forEach(s => s?.dispose());
        this.subscriptions = [];
    }

    warningMessageChanged(newValue: string) {
        this.hasWarning = !!newValue?.trim();
    }

    toggleDetails() {
        this.collapsed = !this.collapsed;
    }

    dismissWarning() {
        this.warningMessage = "";
        this.hasWarning = false;
    }

    get statusClass(): string {
        switch ((this.accent || "").toLowerCase()) {
            case "success":
                return "accent-success";
            case "warning":
                return "accent-warning";
            case "danger":
                return "accent-danger";
            default:
                return "accent-neutral";
        }
    }

    get badgeClass(): string {
        switch ((this.accent || "").toLowerCase()) {
            case "success":
                return "badge-success";
            case "warning":
                return "badge-warning";
            case "danger":
                return "badge-danger";
            default:
                return "badge-neutral";
        }
    }
}